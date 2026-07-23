# AI Prompts — Implementation Phase

> **How to use:** Start a **new Cursor chat** for each prompt. Implement in order. Log every response.

---

## Prompt 1 — Database Schema, Migrations & Seed Data

**When:** First implementation chat.

**@ references:** `data-model.md`, `database/setup-notes.md`, `tool-specific/cursor-workflow/project-context.md`

### Copy into chat:

```
Implement the database layer for the Support Ticket Management System.

Read @data-model.md and @tool-specific/cursor-workflow/project-context.md.

Create:
1. EF Core entity classes (User, Ticket, Comment)
2. DbContext with relationships and configurations
3. Initial EF Core migration in database/schema-or-migrations/
4. Seed data script with 3-5 users and 2-3 sample tickets in database/seed-data/
5. Update database/setup-notes.md with setup steps and .env.example variables

Rules:
- PostgreSQL as primary DB (SQLite acceptable for local dev fallback)
- No hardcoded secrets — use environment variables
- Ticket status defaults to Open
- Follow existing folder structure under src/

Do not implement API controllers yet.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | 2026-07-23 |
| **AI response summary** | Scaffolded `SupportTicket.Api` with EF Core entities (User, Ticket, Comment), Fluent API configurations, `AppDbContext`, PostgreSQL initial migration in `database/schema-or-migrations/`, JSON-only seed via `DbSeeder` reading `database/seed-data/seed-data.json`, env-based Postgres/SQLite provider switch, `.env.example`, and updated `database/setup-notes.md`. No API controllers. |
| **Accepted** | Pending review |
| **Changed** | Seed data JSON-only (no SQL, no HasData, no hardcoded C# rows); SQLite uses `EnsureCreated` instead of Postgres migrations |
| **Rejected** | — |
| **Why** | JSON is the single seed source per user preference; Postgres migrations are Npgsql-specific so SQLite fallback uses model-based schema creation |

---

## Prompt 2 — Status State Machine Service

**When:** After database layer. New chat session.

**@ references:** `api-contract.md`, `requirements-analysis.md`, `design-notes.md`

### Copy into chat:

```
Implement the status state machine for tickets.

Valid transitions ONLY:
- Open → InProgress
- InProgress → Resolved
- Resolved → Closed
- Open → Cancelled
- InProgress → Cancelled

Read @api-contract.md and @requirements-analysis.md.

Create:
1. StatusTransitionService (or equivalent) — single place for all transition rules
2. Method: bool IsValidTransition(currentStatus, newStatus)
3. Method: IEnumerable<string> GetValidNextStatuses(currentStatus)
4. Throw or return Result for invalid transitions (consistent with project error pattern)
5. Unit-testable design (pure logic, no HTTP)

Do not implement controllers yet. Show me the service and any enums.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | 2026-07-23 |
| **AI response summary** | Created `IStatusTransitionService` / `StatusTransitionService`, `ServiceResult`, xUnit matrix tests (25 transition pairs); controller-ready `Program.cs` (`AddControllers`/`MapControllers`); no endpoint controllers |
| **Accepted** | Pending review |
| **Changed** | Used `ServiceResult` instead of exceptions; `GetValidNextStatuses` returns `IEnumerable<string>`; scoped service registration; controllers+services pattern (not minimal API) |
| **Rejected** | Minimal API / endpoint routes in `Program.cs` |
| **Why** | Result pattern for invalid transitions; controller+service architecture per design-notes; `TicketService` maps `ServiceResult` → 400 JSON in Prompt 3 |

---

## Prompt 3 — Backend API (Tickets, Comments, Users)

**When:** After state machine service. New chat session.

**@ references:** `api-contract.md`, `design-notes.md`, `tool-specific/cursor-workflow/cursor-rules-or-instructions.md`

### Copy into chat:

```
Implement the ASP.NET Core Web API for the Support Ticket Management System.

Read @api-contract.md and @tool-specific/cursor-workflow/cursor-rules-or-instructions.md.

Endpoints:
- GET /api/users — list seeded users
- GET /api/tickets — list with ?search= and ?status= query params
- GET /api/tickets/{id} — detail with comments
- POST /api/tickets — create (status defaults to Open)
- PUT /api/tickets/{id} — update fields (NOT status)
- PATCH /api/tickets/{id}/status — status change via StatusTransitionService
- POST /api/tickets/{ticketId}/comments — add comment

Requirements:
- Backend validation on all inputs
- 400 for validation errors and invalid transitions (include error code INVALID_TRANSITION)
- 404 for not found
- Consistent error response JSON shape
- CORS configured for React dev server

Implement controllers, services, DTOs, and wire up EF Core. No frontend yet.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | 2026-07-23 |
| **AI response summary** | Implemented 7 REST endpoints (`UsersController`, `TicketsController`), request/response DTOs, FluentValidation validators (incl. `status` rejection via `JsonExtensionData`), 3 EF Core repositories, `UserService`/`TicketService`/`CommentService`, `ServiceResult<T>`, `ExceptionHandlingMiddleware`, flat `{ "error" }` validation responses, CORS for Vite (`localhost:5173`), Swagger UI + OpenAPI 3 at `/swagger`, port aligned to 5000 |
| **Accepted** | Pending review |
| **Changed** | Swagger/OpenAPI added per user request (was Stretch in original contract); used `Swashbuckle.AspNetCore` 10.x with target-typed `OpenApiInfo`; integration tests deferred to testing phase |
| **Rejected** | — |
| **Why** | Controller+service+repository pattern per design-notes; `TicketService.ChangeStatusAsync` delegates to existing `StatusTransitionService`; manual DTO mapping (no AutoMapper) |

---

## Prompt 4 — React Frontend — Ticket List & Create

**When:** After backend API is working. New chat session.

**@ references:** `ui-flow.md`, `api-contract.md`, `design-notes.md`

### Copy into chat:

```
Implement the React frontend for ticket list and create ticket pages.

Read @ui-flow.md and @api-contract.md.

Pages:
1. Ticket List — table with title, priority, status, assignee, dates
   - Keyword search input (debounced)
   - Status filter dropdown
   - Click row → navigate to detail
   - "Create Ticket" button
2. Create Ticket — form with title, description, priority, assignee, createdBy dropdowns
   - Show validation errors from API

Tech: React + TypeScript + Vite. Use fetch or axios for API calls.
API base URL from environment variable VITE_API_URL.

Show meaningful loading and error states. Do not implement detail page yet.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | 2026-07-23 |
| **AI response summary** | Scaffolded `src/SupportTicket.Web` (Vite + React 19 + TypeScript + MUI + react-router-dom). Implemented fetch-based API client (`VITE_API_URL`), types matching `api-contract.md`, Ticket List page (MUI table, 300ms debounced search, status filter, loading/error/empty states), Create Ticket form (user dropdowns, `localStorage` for `createdBy`, API validation errors), detail-route placeholder for row navigation, `.env.example` |
| **Accepted** | Pending review |
| **Changed** | MUI instead of plain CSS (user preference); redirect to list on create success (detail page deferred to Prompt 5); `TicketDetailPlaceholder` stub at `/tickets/:id` |
| **Rejected** | — |
| **Why** | MUI per user styling choice; list redirect avoids half-built detail flow; placeholder satisfies row-click navigation without Prompt 5 scope |

---

## Prompt 5 — React Frontend — Ticket Detail, Status & Comments

**When:** After list/create pages work. New chat session.

**@ references:** `ui-flow.md`, `api-contract.md`, `requirements-analysis.md`

### Copy into chat:

```
Implement the Ticket Detail page for the Support Ticket Management System.

Read @ui-flow.md and @api-contract.md.

Features:
1. Display all ticket fields
2. Edit title, description, priority, assignee — save via PUT
3. Status change dropdown showing ONLY valid next statuses (use API or client-side rules)
4. On invalid transition — show API error message clearly
5. Comment thread (list) + add comment form
6. 404 handling if ticket not found

This is the signature judgment piece — invalid transitions must be handled clearly in the UI.
Wire to the existing backend API.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | 2026-07-23 |
| **AI response summary** | Extended `types/api.ts` and `ticketsApi.ts` with `TicketDetail`, `Comment`, and four detail endpoints (`getTicket`, `updateTicket`, `changeTicketStatus`, `addComment`). Added `status` to `ApiRequestError` for 404 detection. Built `TicketDetailPage` with overview metadata, PUT edit form (dirty tracking), API-driven status dropdown via `validNextStatuses` with warning `Alert` on transition errors, comment thread + add form, and dedicated 404/invalid-ID states. Replaced `TicketDetailPlaceholder` in routing. |
| **Accepted** | Pending review |
| **Changed** | Status transition errors use `Alert severity="warning"` (not `ErrorBanner`); comments appended locally after POST (no refetch); `FormSection` duplicated inline (not extracted from create page) |
| **Rejected** | — |
| **Why** | Warning alert distinguishes transition failures from validation errors; local comment append avoids full-page reload; inline `FormSection` keeps diff minimal |
