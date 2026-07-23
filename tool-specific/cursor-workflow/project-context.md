# Project Context

## Assessment

**.NET AI Capability Exercise** — Option 1: Backend-Heavy Support Ticket Management System

This is a capability exercise (not a graded test). The goal is to demonstrate thoughtful AI-assisted engineering across the full lifecycle. **Lifecycle artifacts matter as much as the code.**

## Business Context

Internal users manage support tickets: create, update, comment, search, filter, and progress tickets through a defined status lifecycle.

## Stack

| Layer | Technology |
|-------|------------|
| Backend | ASP.NET Core 8 Web API |
| Frontend | React 18 + TypeScript + Vite |
| Database | PostgreSQL (SQLite acceptable for local dev) |
| ORM | Entity Framework Core |
| Tests | xUnit + WebApplicationFactory |

## Core Scope (build this)

- Entities: User (seeded), Ticket, Comment
- Ticket CRUD + comments + search/filter by keyword and status
- **Status state machine** (enforced in backend):
  - Open → InProgress, Cancelled
  - InProgress → Resolved, Cancelled
  - Resolved → Closed
- Backend validation, meaningful UI errors
- Integration tests for state machine
- All lifecycle markdown artifacts

## Out of Scope for Core

- Authentication / JWT
- User management UI
- Pagination, sorting (Stretch)
- Docker, CI, Swagger (Stretch)

## Key Design Decisions

1. **StatusTransitionService** — single place for all transition rules
2. **Separate endpoints** — PUT for fields, PATCH for status
3. **Users seeded** — no user CRUD UI in Core
4. **No secrets in repo** — use `.env.example` only

## Reference Documents

| Doc | Purpose |
|-----|---------|
| `requirements-analysis.md` | What to build |
| `api-contract.md` | API shape |
| `data-model.md` | Database schema |
| `acceptance-criteria.md` | Definition of done |
| `ai-prompts/` | Copy-paste prompts per chat session |

## Repository Structure

```
ai-practical-assessment/
├── src/           # Application code (API + frontend)
├── tests/         # Integration tests
├── database/      # Migrations, seed data, setup notes
├── ai-prompts/    # Prompt history by lifecycle phase
└── [artifacts]    # All root .md files
```
