# Candidate Information

| Field | Value |
|-------|-------|
| **Name** | _[Your name]_ |
| **Role** | _[e.g. Software Engineer]_ |
| **Primary Technology Stack** | .NET 8, ASP.NET Core Web API, React, PostgreSQL |
| **Primary AI Tool Used** | Cursor |
| **Project Option Selected** | Option 1 — Backend-Heavy: Support Ticket Management System |
| **Assessment Start Date** | _[YYYY-MM-DD]_ |
| **Submission Date** | _[YYYY-MM-DD]_ |

## Project Summary

A full-stack support ticket management application where internal users create, update, comment on, search, and progress tickets through an enforced status state machine. Built with AI-assisted engineering across the full lifecycle.

## Tools Used

| Tool | Purpose |
|------|---------|
| Cursor | Primary AI assistant for planning, implementation, testing, debugging, review |
| .NET 8 / ASP.NET Core | Backend API |
| React + TypeScript | Frontend UI |
| PostgreSQL + EF Core | Database persistence |
| xUnit | Integration tests (state machine) |

## Setup Summary

1. Clone the repository.
2. Start PostgreSQL (or use SQLite for local dev).
3. Run database migrations and seed data (see `database/setup-notes.md`).
4. Start the API (`src/` backend) and frontend (`src/` frontend).
5. Open the app in the browser and verify ticket CRUD + status transitions.

See `README.md` for full setup instructions.
