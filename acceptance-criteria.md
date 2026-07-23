# Acceptance Criteria

## Core (11 official assessment criteria)

- [ ] **AC-1 Create ticket via UI** — User can submit a form with title (required), description, priority, assignee (optional), and createdBy; ticket is created with status `Open`
- [ ] **AC-2 View all tickets** — Ticket list loads all tickets from the database (not hardcoded or in-memory only)
- [ ] **AC-3 Ticket detail view** — User can open a ticket and see all fields plus comment thread (oldest first)
- [ ] **AC-4 Update fields and reassign** — User can edit title, description, priority, and assignee via PUT; changes persist; status is not changeable on this endpoint
- [ ] **AC-5 Add comments** — User can add a comment to any ticket (including Closed/Cancelled); comment appears in the thread with author and timestamp
- [ ] **AC-6 Valid transitions only** — Status changes succeed only for valid transitions; all invalid transitions return `400` with `INVALID_TRANSITION`
- [ ] **AC-7 Search and status filter** — Keyword search (title + description, case-insensitive) and status dropdown filter the ticket list; empty results show a friendly message, not an error
- [ ] **AC-8 Data survives restart** — Tickets, comments, and seeded users persist after stopping and restarting the application/database
- [ ] **AC-9 Backend validation** — Invalid records (missing title, bad enums, non-existent user IDs, oversize fields) are rejected with `400`
- [ ] **AC-10 No secrets in repo** — No connection strings, API keys, or passwords committed; `.env.example` documents required variables
- [ ] **AC-11 State-machine integration tests** — Automated tests prove all 5 valid transitions succeed and representative invalid transitions return `400`

## Validation

- [ ] `title` — required, max 200 chars, trimmed; whitespace-only rejected
- [ ] `description` — optional, max 2000 chars
- [ ] `priority` — required enum: `Low` | `Medium` | `High`
- [ ] `createdBy` — required on ticket create and comment create; must reference an existing seeded user
- [ ] `assignedTo` — optional; must reference an existing seeded user if provided; null clears assignee on PUT
- [ ] `status` on create — defaults to `Open`; client cannot set status on POST
- [ ] `status` on PUT — rejected with `400` if included in request body
- [ ] `message` (comment) — required, max 1000 chars, trimmed; whitespace-only rejected
- [ ] `status` query param on list — must be a valid status enum or `400`
- [ ] Foreign keys validated: `assignedTo`, `createdBy`, `ticketId`
- [ ] API returns consistent error response shape (`{ "error": "..." }`; transitions include `"code": "INVALID_TRANSITION"`)

## State Machine

- [ ] Open → InProgress succeeds
- [ ] Open → Cancelled succeeds
- [ ] InProgress → Resolved succeeds
- [ ] InProgress → Cancelled succeeds
- [ ] Resolved → Closed succeeds
- [ ] Same-state transitions rejected (e.g. Open → Open)
- [ ] Skip transitions rejected (e.g. Open → Closed, Open → Resolved)
- [ ] Backward transitions rejected (e.g. InProgress → Open)
- [ ] Terminal-state transitions rejected (Closed/Cancelled → any)
- [ ] Cancel after Resolved rejected (Resolved → Cancelled)
- [ ] UI status dropdown shows only valid next statuses for current state

## Error Handling

- [ ] `400` for validation errors and invalid transitions
- [ ] `404` for missing ticket on GET, PUT, PATCH, or comment POST
- [ ] `500` handled gracefully without leaking stack traces to client
- [ ] UI displays API error messages for validation failures
- [ ] UI displays server message for invalid status transitions
- [ ] UI shows "Ticket not found" page with link back to list on `404`
- [ ] UI shows "No tickets match your search" for empty search/filter results
- [ ] Network failure shows a user-friendly connection error

## Testing

- [ ] Integration tests: all 5 valid transitions succeed
- [ ] Integration tests: invalid transitions return `400` with `INVALID_TRANSITION`
- [ ] Integration tests: same-state and terminal-state transitions rejected
- [ ] Integration tests: status field rejected on POST and PUT
- [ ] Integration tests: validation errors for missing title, bad priority, non-existent user
- [ ] Tests run via documented command in README

## Documentation

- [ ] README with setup and run instructions (backend + frontend + database)
- [ ] Database setup documented in `database/setup-notes.md`
- [ ] All lifecycle artifacts present in repository
- [ ] Full prompt history in `ai-prompts/`

## Users (seeded, read-only)

- [ ] 3–5 users seeded in database with id, name, email, role
- [ ] `GET /api/users` returns seeded users for UI dropdowns
- [ ] No user create/update/delete endpoints or UI in Core
