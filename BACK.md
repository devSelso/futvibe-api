# BACK.md — Futvibe API

## Stack

| Camada | Tecnologia |
|--------|-----------|
| Runtime | .NET 9 / ASP.NET Core 9 |
| ORM | EF Core 9 + Npgsql |
| Banco | PostgreSQL (Supabase) |
| Auth | JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer) |
| CQRS | MediatR 14 |
| Validação | FluentValidation 12 |
| Hash | BCrypt.Net-Next |
| Docs | Swashbuckle 6.9 (Swagger UI) |
| Deploy | Railway (Docker) |

---

## Arquitetura — Clean Architecture

```
Futvibe.Domain          ← entidades, interfaces, enums, exceções
Futvibe.Application     ← CQRS handlers, DTOs, mappers, behaviors
Futvibe.Infrastructure  ← EF Core, repositórios, JWT service
Futvibe.WebApi          ← controllers, middleware, Program.cs
```

Dependência: WebApi → Application + Infrastructure → Domain

---

## Endpoints

| Método | Rota | Auth | Descrição |
|--------|------|------|-----------|
| POST | /api/auth/login | — | Login |
| GET | /api/users/me | ✓ | Usuário logado |
| GET | /api/users/{id} | — | Usuário por ID |
| PUT | /api/users/me | ✓ | Atualizar perfil |
| GET | /api/matches | — | Feed (filtros: level, paid, page, limit) |
| GET | /api/matches/me | ✓ | Partidas do usuário |
| GET | /api/matches/{id} | — | Partida por ID |
| POST | /api/matches | ✓ | Criar partida |
| POST | /api/matches/{id}/join | ✓ | Entrar em partida |
| PATCH | /api/matches/{matchId}/participants/{userId} | ✓ | Aprovar/rejeitar participante |
| GET | /api/banners | — | Banners ativos |

---

## Fases de Implementação

### [x] B1 — Estrutura do Projeto
- Solução com 4 projetos (Domain, Application, Infrastructure, WebApi)
- Referências entre projetos configuradas
- Packages base instalados

### [x] B2 — Domain
- Entidades: `User`, `Match`, `Participant`, `Banner`
- Enums: `MatchLevel`, `MatchVisibility`, `ParticipantStatus`
- Exceções: `NotFoundException`, `ForbiddenException`, `BusinessException`
- Interfaces de repositório: `IUserRepository`, `IMatchRepository`, `IBannerRepository`
- Interface de serviço: `IJwtService`
- Lógica de negócio em `Match`: `DetermineStatusForNewJoiner()`, `AddParticipant()`, `UpdateParticipantStatus()`
- `_participants` como campo privado + `IReadOnlyCollection<Participant>` público

### [x] B3 — Application
- DTOs: `UserDto`, `ParticipantDto`, `MatchDto`, `BannerDto`
- Mappers estáticos: `MatchMapper.ToDto()`, `UserMapper.ToDto()`
- `ValidationBehavior` + `LoggingBehavior` (MediatR pipeline)
- Auth: `LoginCommand`, `LoginCommandHandler`, `LoginResult`
- Matches: GetMatches, GetMatchById, GetUserMatches, CreateMatch, JoinMatch, UpdateParticipantStatus
- Users: GetCurrentUser, GetUserById, UpdateProfile
- Banners: GetBanners
- `DependencyInjection.AddApplication()`

### [x] B4 — Infrastructure
- `FutvibeDbContext` com 4 DbSets
- Configurações EF Core (snake_case, enums como string, backing field `_participants`)
- Repositórios: `UserRepository`, `MatchRepository`, `BannerRepository`
- `JwtService` — tokens 30 dias, claim `NameIdentifier` = userId
- `DependencyInjection.AddInfrastructure()`

### [x] B5 — Migrations
- Migration inicial criada via `dotnet ef migrations add InitialCreate`
- Tabelas: `users`, `matches`, `participants`, `banners`
- Migrations rodam no startup via `db.Database.MigrateAsync()`

### [x] B6 — Auth Endpoint
- `POST /api/auth/login` funcional
- BCrypt.Verify + JWT generation
- `LoginResult` retorna token + UserDto

### [x] B7 — WebApi completo
- `ExceptionHandlerMiddleware` — mapeia exceções de domínio para HTTP status
- `ClaimsPrincipalExtensions.GetUserId()` — extrai userId do JWT
- Controllers: `AuthController`, `UsersController`, `MatchesController`, `BannersController`
- `Program.cs` — JWT Bearer, CORS, Swagger com Bearer security, `MigrateAsync` no startup
- `appsettings.json` (dev) + `appsettings.Production.json` (env vars)
- `Dockerfile` multi-stage (SDK 9 build → aspnet:9 runtime, porta 8080)
- Build: **0 erros, 0 avisos**
- Fix aplicado: Swashbuckle 6.9.0 (OpenApi 1.x) em vez de 10.x (OpenApi 2.x quebrou namespace)

### [x] B8 — Seed de dados
- `DataSeeder.SeedAsync()` em `Futvibe.Infrastructure/Persistence/DataSeeder.cs`
- Idempotente: pula se `Users` não estiver vazio
- 5 usuários (espelhando mocks): Rafael, Juliana, Bruno, Camila, Diego
- Login principal: `teste@futvibe.app` / `123456`
- 7 partidas com participantes variados (host + confirmed + pending + waitlist)
- 3 banners (iFood, Uber, Kirra Fitness)
- Chamado em `Program.cs` após `MigrateAsync()`
- Build: **0 erros, 0 avisos**

### [x] B9 — Adaptação do frontend
- `api/client.ts` — instância Axios, interceptor JWT (lê localStorage, skip no server)
- `api/auth.ts` — `apiLogin()` → POST /api/auth/login
- `api/matches.ts` — calls reais: getMatches, getMatchById, getMyMatches, createMatch, joinMatch, updateParticipantStatus
- `api/users.ts` — calls reais: getCurrentUser, getUserById, updateProfile
- `api/banners.ts` — call real: getBanners
- `lib/auth.ts` — armazena JWT (`futvibe_token`) + userId (`futvibe_user_id`) + cookie para SSR
- `features/matches/types` — `Participant` ganhou campo `user?: User` (embeds do backend)
- Services de feature apontam para camada `api/` (mocks removidos do fluxo)
- `joinMatch` e `fetchUserMatches` removeram parâmetro `userId` (JWT autentica)
- `match-actions.tsx` — removido `currentUserId` (não mais necessário)
- `use-current-user.ts` — usa `GET /api/users/me` em vez de mock
- `use-update-profile.ts` — usa `PUT /api/users/me` em vez de localStorage
- `app/(main)/match/[id]/page.tsx` — usa `user` embedded em cada participante (sem N+1 fetches)
- `.env.local` — `NEXT_PUBLIC_API_URL=http://localhost:5000`
- TypeScript: 0 erros reais (apenas artefato stale `.next/types`)

### [ ] B10 — Deploy (aguardando aprovação)
- Supabase: criar projeto + connection string
- Railway: conectar repo, configurar variáveis de ambiente
- Vercel: configurar `NEXT_PUBLIC_API_URL` apontando para Railway

### [ ] B10 — Deploy (aguardando aprovação)
- Supabase: criar projeto + connection string
- Railway: conectar repo, configurar variáveis de ambiente
- Vercel: configurar `NEXT_PUBLIC_API_URL` apontando para Railway

---

## Variáveis de Ambiente (Produção)

```
ConnectionStrings__Default=Host=...;Port=6543;Database=postgres;Username=postgres;Password=...;Pooling=false
Jwt__Secret=<32+ chars>
AllowedOrigins=https://futvibe.vercel.app
```

> Supabase Transaction mode (porta 6543) exige `Pooling=false` para evitar conflito com pool interno do Npgsql.
