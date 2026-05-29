# Futvibe API — CLAUDE.md

## Project

Football match-making platform. Players find, create, and join pickup matches.

## Architecture

Clean Architecture, 4 layers:

```
src/
├── Futvibe.Domain/          # Entities, interfaces, enums, domain exceptions
├── Futvibe.Application/     # CQRS handlers, validators, DTOs, pipeline behaviors
├── Futvibe.Infrastructure/  # EF Core repositories, JWT service, data seeding
└── Futvibe.WebApi/          # Controllers, middleware, Program.cs
```

**Patterns:** CQRS via MediatR, Repository, FluentValidation pipeline behavior, DDD factory methods.

## Tech Stack

- .NET 9.0 / ASP.NET Core Web API
- PostgreSQL on Supabase (EF Core 9 + Npgsql)
- MediatR 14 — all controller actions delegate to `IMediator`
- FluentValidation 12 — auto-runs via `ValidationBehavior<,>`
- JWT Bearer auth (30-day tokens, BCrypt password hashing)
- Swagger (dev only)

## Domain Models

| Entity | Key Fields |
|--------|-----------|
| User | Id, Name, Email, PasswordHash, Avatar, Bio, Level, PresenceScore, MatchesPlayed |
| Match | Id, Title, Location, Date, Time, Level, PricePerPlayer, MaxPlayers, Visibility, HostId |
| Participant | MatchId + UserId (composite PK), Status |
| Banner | Id, ImageUrl, Alt, Href, IsActive, DisplayOrder |

**Enums:**
- `MatchLevel`: Beginner, Intermediate, Advanced
- `MatchVisibility`: Public, Private, Hybrid
- `ParticipantStatus`: Host, Confirmed, Pending, Rejected, Waitlist

## Endpoints

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/api/auth/login` | — | Login → JWT |
| GET | `/api/users/me` | ✓ | Current user |
| GET | `/api/users/{id}` | — | User by ID |
| PUT | `/api/users/me` | ✓ | Update profile |
| GET | `/api/matches` | — | List matches (filter: level, paid, page, limit) |
| GET | `/api/matches/me` | ✓ | User's matches |
| GET | `/api/matches/{id}` | — | Match detail |
| POST | `/api/matches` | ✓ | Create match |
| POST | `/api/matches/{id}/join` | ✓ | Join match |
| PATCH | `/api/matches/{matchId}/participants/{userId}` | ✓ | Update participant status |
| GET | `/api/banners` | — | Active promotional banners |

## Auth

JWT Bearer. Token carries: `NameIdentifier` (userId), `Email`, `Name`.

Extract userId in controllers via `User.GetUserId()` (`ClaimsPrincipalExtensions`).

Protected endpoints use `[Authorize]` attribute.

**Dev credentials (seeded):** `teste@futvibe.app` / `123456` (or any `@futvibe.app` seed user).

## Database

PostgreSQL (Supabase). Tables: `users`, `matches`, `participants`, `banners`.

Migrations run automatically on startup. Seed data applied if no users exist (5 users, 7 matches, 3 banners).

## Error Handling

`ExceptionHandlerMiddleware` maps domain exceptions to HTTP codes:

| Exception | HTTP |
|-----------|------|
| NotFoundException | 404 |
| ForbiddenException | 403 |
| BusinessException | 422 |
| ValidationException | 400 |
| Unhandled | 500 |

Responses: structured JSON, Portuguese error messages, camelCase.

## Adding New Features

1. **Domain** — add entity/enum if needed
2. **Application** — create `Command` or `Query` + `Handler` + `Validator` under `src/Futvibe.Application/{Feature}/`
3. **Infrastructure** — add repo method if needed
4. **WebApi** — add controller action, delegate to `IMediator`

Never put business logic in controllers or handlers — keep it in domain entities.

## Config

| Key | Dev value | Prod source |
|-----|-----------|-------------|
| ConnectionStrings__Default | Supabase Postgres URL | env var |
| Jwt__Secret | `dev-secret-change-in-production-min-32-chars!!` | env var |
| AllowedOrigins | `http://localhost:3000` | env var |

## Deploy

- **Railway** — Docker deploy. Port 8080.
- **Supabase** — PostgreSQL. Use Transaction mode port `6543` + `Pooling=false` in connection string.
- **Vercel** — Frontend. Set `NEXT_PUBLIC_API_URL` to Railway URL. Prod CORS: `https://futvibe.vercel.app`.

Dev API: `http://localhost:5122`. Swagger: `http://localhost:5122/swagger`.

```bash
dotnet run --project src/Futvibe.WebApi
```

## No Tests

No test projects exist. When adding, use xUnit + Moq. Integration tests should target real DB (not mocked).
