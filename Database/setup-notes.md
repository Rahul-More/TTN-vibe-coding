# Database Setup Notes

## Database Choice

**Primary:** PostgreSQL  
**Local dev fallback:** SQLite (optional — document if used)

## Prerequisites

- PostgreSQL 14+ installed and running
- .NET 8 SDK
- EF Core CLI: `dotnet tool install --global dotnet-ef`

## Environment Variables

Create a `.env` file (do **not** commit) based on `.env.example`:

```env
# .env.example — copy to .env and fill in values

# PostgreSQL
ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=ticketdb;Username=postgres;Password=YOUR_PASSWORD

# API
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://localhost:5000

# Frontend
VITE_API_URL=http://localhost:5000/api
```

## Setup Steps

### 1. Create database

```sql
CREATE DATABASE ticketdb;
```

### 2. Apply migrations

```bash
cd src/[YourApiProject]
dotnet ef database update
```

Migrations are in `database/schema-or-migrations/` (or generated under the API project — copy/symlink as needed).

### 3. Seed data

Seed runs automatically on startup, or run manually:

```bash
# If you have a seed command/script, document it here
dotnet run --project src/[YourApiProject] -- seed
```

Seed data files: `database/seed-data/`

### 4. Verify

```bash
# Connect to DB and check tables
psql -d ticketdb -c "SELECT * FROM \"Users\";"
psql -d ticketdb -c "SELECT * FROM \"Tickets\";"
```

## Schema Overview

| Table | Purpose |
|-------|---------|
| Users | Seeded internal users |
| Tickets | Support tickets |
| Comments | Ticket comments |

See `data-model.md` for full column definitions.

## Troubleshooting

| Issue | Fix |
|-------|-----|
| Connection refused | Check PostgreSQL is running on port 5432 |
| Migration pending | Run `dotnet ef database update` |
| FK constraint on seed | Seed users before tickets |
| Password auth failed | Verify `.env` connection string |

## Files in This Folder

```
database/
├── schema-or-migrations/   # EF Core migration files (generated during implementation)
├── seed-data/              # SQL or JSON seed scripts
└── setup-notes.md          # This file
```
