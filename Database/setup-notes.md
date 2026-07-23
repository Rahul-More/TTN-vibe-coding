# Database Setup Notes

## Database Choice

**PostgreSQL** — required for local development and runtime.

## Prerequisites

- .NET 8 SDK
- EF Core CLI: `dotnet tool install --global dotnet-ef`
- PostgreSQL 14+

## Environment Variables

Copy [`.env.example`](../.env.example) to `.env` at the repo root (do **not** commit `.env`):

```env
ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=ticketdb;Username=postgres;Password=password
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://localhost:5000
VITE_API_URL=http://localhost:5000/api
```

### PowerShell (session)

```powershell
$env:ConnectionStrings__DefaultConnection = "Host=localhost;Port=5432;Database=ticketdb;Username=postgres;Password=password"
$env:ASPNETCORE_ENVIRONMENT = "Development"
```

### Bash

```bash
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=ticketdb;Username=postgres;Password=password"
export ASPNETCORE_ENVIRONMENT=Development
```

## Setup Steps

### 1. Create database

```sql
CREATE DATABASE ticketdb;
```

### 2. Apply migrations

```bash
dotnet ef database update --project src/SupportTicket.Api --startup-project src/SupportTicket.Api
```

Migrations live in [`database/schema-or-migrations/`](schema-or-migrations/) and are compiled into `SupportTicket.Api` via the project file.

### 3. Seed data

Seed runs automatically on API startup from [`database/seed-data/seed-data.json`](seed-data/seed-data.json).

- **Mechanism:** `DbSeeder` reads JSON only (no SQL scripts, no `HasData` in migrations)
- **Idempotent:** Skips if users already exist
- **Content:** 5 users, 3 sample tickets, 4 comments

To re-seed from scratch, drop/recreate the database and restart the API.

### 4. Run the API

```bash
cd src/SupportTicket.Api
dotnet run
```

On first run: migrations and JSON seed execute automatically.

### 5. Verify

```bash
psql -d ticketdb -c 'SELECT "Id", "Name", "Email" FROM "Users";'
psql -d ticketdb -c 'SELECT "Id", "Title", "Status" FROM "Tickets";'
```

## Schema Overview

| Table | Purpose |
|-------|---------|
| Users | Seeded internal users |
| Tickets | Support tickets |
| Comments | Ticket comments |

See [`data-model.md`](../data-model.md) for full column definitions, indexes, and FK rules.

## Troubleshooting

| Issue | Fix |
|-------|-----|
| Connection refused | Check PostgreSQL is running on port 5432 |
| Password auth failed | Verify connection string in `.env` or environment |
| Migration pending | Run `dotnet ef database update --project src/SupportTicket.Api` |
| FK constraint on seed | Ensure `seed-data.json` lists users before tickets/comments |
| Seed file not found | Run API from repo with `src/SupportTicket.Api` as content root; JSON is at `database/seed-data/seed-data.json` |

## Files in This Folder

```
database/
├── schema-or-migrations/   # EF Core migration files (InitialCreate)
├── seed-data/
│   └── seed-data.json      # Single source of truth for seed data
└── setup-notes.md          # This file
```
