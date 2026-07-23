# Acceptance Criteria

## Core Features (11 official assessment criteria)

- [ ] **AC-1 Create ticket via UI** — User submits a form with `title` (required), `description` (optional), `priority` (required), `assignedTo` (optional), and `createdBy` (required, from seeded-user dropdown); ticket is created via `POST /api/tickets` with default status `Open`; client cannot set `status` on create
- [ ] **AC-2 View all tickets** — Ticket list loads all tickets from the real database via `GET /api/tickets` (not hardcoded or in-memory only); each row shows key fields (title, priority, status, assignee, timestamps)
- [ ] **AC-3 Ticket detail view** — User opens a ticket via `GET /api/tickets/{id}` and sees full ticket fields plus embedded comment thread sorted **oldest first** (chronological)
- [ ] **AC-4 Update fields and reassign** — User edits `title`, `description`, `priority`, and `assignedTo` via `PUT /api/tickets/{id}`; changes persist; `status` is **not** changeable on this endpoint (rejected with `400` if sent); field updates allowed on Closed/Cancelled tickets (status remains locked)
- [ ] **AC-5 Add comments** — User adds a comment via `POST /api/tickets/{ticketId}/comments` with `message` and `createdBy`; comment appears in the thread with author and timestamp; comments allowed on Closed/Cancelled tickets; comments are append-only (no edit/delete in Core)
- [ ] **AC-6 Valid transitions only** — Status changes only via `PATCH /api/tickets/{id}/status`; all 5 valid transitions succeed; all other transitions (including same-state no-ops and terminal-state moves) return `400` with `code: "INVALID_TRANSITION"`; UI status control shows only valid next statuses for the current state
- [ ] **AC-7 Search and status filter** — `GET /api/tickets` supports optional `search` (case-insensitive keyword in title + description) and `status` filter; combined filters work; empty results return `200` with `[]` and UI shows a friendly "no results" message (not an error)
- [ ] **AC-8 Data survives restart** — Tickets, comments, and seeded users persist after stopping and restarting the application and database (real DB, not in-memory only)
- [ ] **AC-9 Backend validation** — Invalid records rejected server-side with `400` (required fields, max lengths, enum values, foreign-key existence, invalid `status` query param); consistent error response shape
- [ ] **AC-10 No secrets in repo** — No connection strings, API keys, or passwords committed; configuration via environment variables; `.env.example` documents required variables
- [ ] **AC-11 State-machine integration tests** — Automated integration tests (e.g. `WebApplicationFactory` + test DB) prove all 5 valid transitions succeed and invalid transitions return `400` with `INVALID_TRANSITION`; tests run via documented command (`dotnet test`)

### Supporting Core scope (not separate AC numbers)

- [ ] **Seeded users** — 3–5 users in DB with `id`, `name`, `email`, `role`; `GET /api/users` returns them for assignee and `createdBy` dropdowns
- [ ] **No user management** — No user create/update/delete API endpoints or UI in Core
- [ ] **No authentication** — No login/JWT in Core; `createdBy` selected from dropdown (trusted client)
- [ ] **No ticket delete** — Tickets are permanent; `Cancelled` is the abandon path
- [ ] **Status logic centralized** — `StatusTransitionService` (or equivalent) is the single enforcement point for transitions
- [ ] **Timestamps** — `createdAt` and `updatedAt` stored in UTC; `updatedAt` updates on field changes and status changes
- [ ] **Explicitly out of Core** — No pagination, sorting, Docker, CI, or Swagger unless Stretch is attempted separately

---

## Validation Rules

### Ticket fields

- [ ] `title` — required on create and update; max 200 characters; trimmed; whitespace-only rejected (`400`)
- [ ] `description` — optional; max 2000 characters
- [ ] `priority` — required enum: `Low` | `Medium` | `High`; invalid values rejected (`400`)
- [ ] `createdBy` — required on ticket create; must reference an existing seeded user (`400` if not)
- [ ] `assignedTo` — optional; must reference an existing seeded user if provided; `null`/omitted on create = unassigned
- [ ] `status` on `POST /api/tickets` — defaults to `Open`; client cannot set initial status (`400` if `status` sent)
- [ ] `status` on `PUT /api/tickets/{id}` — rejected with `400` if included in body; forces use of PATCH endpoint
- [ ] `PUT` semantics — full replace of updatable fields; omitted nullable `assignedTo` may clear assignee (per Decision #12)

### Comment fields

- [ ] `message` — required; max 1000 characters; trimmed; whitespace-only rejected (`400`)
- [ ] `createdBy` — required; must reference an existing seeded user (`400` if not)
- [ ] `ticketId` — must reference an existing ticket (`404` if ticket missing)

### List query params

- [ ] `search` — optional string; case-insensitive match on title and description
- [ ] `status` — optional; must be a valid status enum value or `400` (strict validation, Decision #8)

### Foreign keys and enums

- [ ] `assignedTo`, `createdBy` (ticket and comment), and `ticketId` validated against existing records
- [ ] Status enum values: `Open`, `InProgress`, `Resolved`, `Closed`, `Cancelled`

---

## Error Handling Expectations

### HTTP status codes

- [ ] `400` — validation errors (missing/invalid fields, bad enums, non-existent user IDs, oversize fields, invalid `status` query param)
- [ ] `400` — invalid status transitions with body `{ "error": "<message>", "code": "INVALID_TRANSITION" }`
- [ ] `400` — `status` field rejected on `POST` and `PUT` ticket endpoints
- [ ] `404` — missing ticket on `GET`, `PUT`, `PATCH /status`, and comment `POST` with `{ "error": "Ticket not found" }`
- [ ] `201` — successful ticket and comment creation
- [ ] `200` — successful read, update, status change, and list (including empty list)
- [ ] `500` — handled gracefully; no stack traces or internal details leaked to client

### Consistent error shape

- [ ] Validation and not-found errors use `{ "error": "<human-readable message>" }`
- [ ] Invalid transitions always include `"code": "INVALID_TRANSITION"`

### UI error surfacing

- [ ] Validation errors from API displayed meaningfully on create/update/comment forms
- [ ] Invalid transition message displayed when status change fails
- [ ] Dedicated not-found state for missing ticket (e.g. message + link back to list)
- [ ] Empty search/filter results show friendly empty state (not treated as error)
- [ ] Network or server failures show user-friendly connection/error message

### Edge cases (from requirements analysis)

- [ ] Same-state transition (e.g. Open → Open) → `400` `INVALID_TRANSITION`
- [ ] Skip transitions (e.g. Open → Closed, Open → Resolved, InProgress → Closed) → `400`
- [ ] Backward transitions (e.g. InProgress → Open) → `400`
- [ ] Transitions from terminal states (Closed/Cancelled → any) → `400`
- [ ] Resolved → Cancelled → `400`
- [ ] Concurrent status updates → last-write-wins (documented behavior, no crash)

---

## Testing Requirements (state machine integration tests)

### Required integration test infrastructure

- [ ] Tests use `WebApplicationFactory` (or equivalent) with a real/test database (not in-memory mock of business rules)
- [ ] Tests are runnable via `dotnet test` from documented path (e.g. `tests/`)
- [ ] Results documented in `test-results.md`

### Valid transitions (all 5 must pass)

- [ ] Open → InProgress → `200`, status persisted
- [ ] Open → Cancelled → `200`, status persisted
- [ ] InProgress → Resolved → `200`, status persisted
- [ ] InProgress → Cancelled → `200`, status persisted
- [ ] Resolved → Closed → `200`, status persisted

### Invalid transitions (representative minimum; full matrix = 20 invalid cases)

- [ ] Same-state: Open → Open → `400` `INVALID_TRANSITION`
- [ ] Skip: Open → Closed → `400`
- [ ] Skip: Open → Resolved → `400`
- [ ] Skip: InProgress → Closed → `400`
- [ ] Backward: InProgress → Open → `400`
- [ ] Post-resolve cancel: Resolved → Cancelled → `400`
- [ ] Terminal: Closed → Open (and/or other terminal → any) → `400`
- [ ] Terminal: Cancelled → InProgress → `400`

### Status endpoint isolation

- [ ] `status` in `POST /api/tickets` body → `400`
- [ ] `status` in `PUT /api/tickets/{id}` body → `400`

### Recommended additional integration tests

- [ ] Create ticket with missing/blank title → `400`
- [ ] Create ticket with invalid priority → `400`
- [ ] Create ticket with non-existent `createdBy` → `400`
- [ ] `GET /api/tickets/{id}` for non-existent ID → `404`
- [ ] Comment on non-existent ticket → `404`
- [ ] List with invalid `status` query param → `400`
- [ ] Update fields on Closed ticket succeeds (fields only; status unchanged)

---

## Documentation Requirements

- [ ] **README.md** — local setup and run instructions for backend, frontend, and database; prerequisites listed; test command documented
- [ ] **`.env.example`** — all required environment variables documented (no real secrets)
- [ ] **`database/setup-notes.md`** — database creation, migrations, and seed data instructions
- [ ] **`api-contract.md`** — endpoint shapes align with implementation (or note intentional deviations)
- [ ] **`test-results.md`** — output or summary of latest `dotnet test` run for Core submission
- [ ] **Lifecycle artifacts present** — requirements-analysis, acceptance-criteria, implementation-plan, design-notes, test-strategy, and other assessment artifacts at repo root per participant guide
- [ ] **`ai-prompts/`** — full prompt history retained for Part A workflow evidence
- [ ] **No secrets in repository** — verified before submission (reinforces AC-10)
