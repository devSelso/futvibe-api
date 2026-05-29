# Futvibe — Front-End Integration Guide

## Base URL

| Env | URL |
|-----|-----|
| Dev | `http://localhost:5122` |
| Docker | `http://localhost:8080` |
| Prod | TBD (configure via env var) |

CORS is configured to allow `http://localhost:3000` in dev.

## Authentication

**Login:**
```http
POST /api/auth/login
Content-Type: application/json

{ "email": "rafael@example.com", "password": "123456" }
```

**Response:**
```json
{
  "token": "<jwt>",
  "user": {
    "id": "uuid",
    "name": "Rafael",
    "email": "rafael@example.com",
    "avatar": null,
    "bio": null,
    "level": "intermediate",
    "presenceScore": 0,
    "matchesPlayed": 0
  }
}
```

Store JWT in memory or `localStorage`. Send on every protected request:
```
Authorization: Bearer <token>
```

Token expiry: **30 days**.

No refresh token endpoint — on 401, redirect to login.

## Enums (serialized as lowercase strings)

**MatchLevel:** `"beginner"` | `"intermediate"` | `"advanced"`

**MatchVisibility:** `"public"` | `"private"` | `"hybrid"`

**ParticipantStatus:** `"host"` | `"confirmed"` | `"pending"` | `"rejected"` | `"waitlist"`

## Endpoints

### Users

**Get current user** (requires auth):
```http
GET /api/users/me
```

**Get user by ID:**
```http
GET /api/users/{id}
```

**Update profile** (requires auth):
```http
PUT /api/users/me
Content-Type: application/json

{ "name": "string", "bio": "string|null", "level": "beginner|intermediate|advanced" }
```
Returns updated `UserDto`.

---

### Matches

**List matches:**
```http
GET /api/matches?level=beginner&paid=false&page=1&limit=10
```

All params optional. `paid=true` filters matches with `pricePerPlayer > 0`.

**Response:**
```json
[
  {
    "id": "uuid",
    "title": "Pelada do Bairro",
    "location": "Campo Central",
    "date": "2026-06-01",
    "time": "18:00:00",
    "level": "intermediate",
    "pricePerPlayer": 0,
    "maxPlayers": 14,
    "visibility": "public",
    "hostId": "uuid",
    "host": { "id": "uuid", "name": "Rafael", "avatar": null },
    "participantCount": 5,
    "participants": [ { "userId": "uuid", "status": "confirmed", "user": {...} } ],
    "createdAt": "2026-05-28T01:00:00Z"
  }
]
```

**Get match by ID:**
```http
GET /api/matches/{id}
```

**Get user's matches** (requires auth):
```http
GET /api/matches/me
```

**Create match** (requires auth):
```http
POST /api/matches
Content-Type: application/json

{
  "title": "string",          // max 200
  "location": "string",       // max 300
  "date": "YYYY-MM-DD",
  "time": "HH:MM:SS",
  "level": "beginner|intermediate|advanced",
  "pricePerPlayer": 0,        // decimal, default 0
  "maxPlayers": 14,           // int
  "visibility": "public|private|hybrid"
}
```

Returns `201 Created` + match object. Creator auto-added as `host` participant.

**Join match** (requires auth):
```http
POST /api/matches/{id}/join
```

Returns `204 No Content`. Status assigned automatically by `DetermineStatusForNewJoiner()`:

| Visibility | Spots available? | Status |
|-----------|-----------------|--------|
| `private` | yes | `confirmed` |
| `hybrid` | yes | `confirmed` |
| `public` | yes | `pending` |
| any | no (full) | `waitlist` |

"Full" = confirmed + host count ≥ `maxPlayers`.

**Update participant status** (requires auth, host only):
```http
PATCH /api/matches/{matchId}/participants/{userId}
Content-Type: application/json

{ "status": "confirmed|pending|rejected|waitlist" }
```

Returns `204 No Content`.

---

### Banners

**Get active banners:**
```http
GET /api/banners
```

```json
[
  {
    "id": "uuid",
    "imageUrl": "https://...",
    "alt": "iFood",
    "href": "https://ifood.com.br",
    "displayOrder": 1
  }
]
```

Ordered by `displayOrder`. Use for homepage carousel/ads.

## Error Responses

All errors return:
```json
{
  "error": "Mensagem de erro em português",
  "details": []
}
```

| HTTP | Meaning |
|------|---------|
| 400 | Validation error (bad input) |
| 401 | Missing or invalid JWT |
| 403 | Action not allowed (e.g., non-host updating participant) |
| 404 | Resource not found |
| 422 | Business rule violation (e.g., match full, already joined) |
| 500 | Server error |

## Response Format

- All JSON keys are **camelCase**
- Dates: `"YYYY-MM-DD"` (DateOnly)
- Times: `"HH:MM:SS"` (TimeOnly)
- Timestamps: ISO 8601 UTC `"2026-05-28T01:00:00Z"`
- Enums: lowercase strings
- UUIDs: lowercase hyphenated strings

## Dev Seed Data

Users (password `123456` for all):
- `teste@futvibe.app` — Rafael Costa, Advanced
- `juliana@futvibe.app` — Juliana Melo, Intermediate
- `bruno@futvibe.app` — Bruno Alves, Beginner
- `camila@futvibe.app` — Camila Torres, Advanced
- `diego@futvibe.app` — Diego Nunes, Intermediate

7 matches seeded at various levels/visibility. 3 banners (iFood, Uber, Kirra Fitness).

## Deploy

| Service | Role |
|---------|------|
| Railway | API deploy via Dockerfile |
| Supabase | PostgreSQL (port 6543, `Pooling=false`) |
| Vercel | Frontend — set `NEXT_PUBLIC_API_URL` to Railway URL |

Prod CORS origin: `https://futvibe.vercel.app`

## Key UX Notes

- `visibility: "hybrid"` — joiners auto-confirmed if spots available. No payment distinction in join logic.
- `visibility: "private"` — joiners auto-confirmed if spots available. Host controls via PATCH endpoint.
- `visibility: "public"` — joiners start as `pending`. Host must confirm each participant.
- `presenceScore` — future gamification field, not yet computed dynamically.
- `matchesPlayed` — not yet auto-incremented on join (future feature).
- No user registration endpoint yet — only login. New users created via DB seed or future `/api/auth/register`.
- Swagger UI available at `/swagger` when API runs in development mode.
