# Test Strategy — Support Ticket Management System (Core)

Design-phase test strategy for the Core submission. Aligned with `requirements-analysis.md`, `api-contract.md`, and `acceptance-criteria.md` (AC-6, AC-11).

**No test code in this document** — implementation follows in `tests/` during the build phase.

---

## 1. Purpose and scope

### Core mandatory tier

Integration tests proving the **ticket status state machine** via HTTP:

- All **5 valid transitions** must succeed (`200`, status persisted)
- **Invalid transitions** must return `400` with `code: "INVALID_TRANSITION"`
- Minimum for submission prompt: **5 valid + at least 4 invalid** integration tests
- Runtime requirement (AC-6): **all 20 invalid transitions** must be rejected by the backend; tests should cover the full matrix via parameterized cases or equivalent

### Enforcement point

Status rules live in a single `StatusTransitionService` (see `design-notes.md`). Integration tests exercise the full stack (controller → service → DB) through `PATCH /api/tickets/{id}/status`.

### Test tiers

| Tier | Core? | Focus |
|------|-------|-------|
| Integration (API) | **Required** | State machine, validation, 404, status endpoint isolation |
| Unit | Optional Stretch | Full 25-pair transition matrix on `StatusTransitionService` without HTTP |
| Component (React) | Out of Core | Status dropdown, error toasts — manual demo sufficient |
| E2E (Playwright) | Out of Core | Full UI flows — not required for assessment |

---

## 2. Integration test infrastructure

### Framework and project layout

| Item | Recommendation |
|------|----------------|
| Test framework | xUnit |
| HTTP testing | `Microsoft.AspNetCore.Mvc.Testing` |
| Project path | `tests/SupportTicket.Api.Tests/` (name TBD at implementation) |
| Run command | `dotnet test` from `tests/` |

### WebApplicationFactory setup

Use a custom `WebApplicationFactory<Program>` that overrides `ConfigureWebHost` to:

1. Set `ASPNETCORE_ENVIRONMENT=Testing`
2. Replace the production connection string with a **test database** connection string (environment variable or `appsettings.Testing.json`)
3. Apply EF Core migrations and seed 3–5 users before the test suite runs
4. Register the factory as `IClassFixture<CustomWebApplicationFactory>` or `ICollectionFixture` so the host is shared across tests in a collection

### Test database options

| Option | Pros | Cons |
|--------|------|------|
| **Dedicated PostgreSQL test DB** (preferred) | Matches production; satisfies AC-11 | Requires local Postgres or shared CI service |
| **Testcontainers PostgreSQL** | Isolated per run; CI-friendly | Slower startup; Docker required |
| EF Core InMemory | Fast | **Not recommended** for state-machine tests — does not prove real persistence or PostgreSQL behavior |

**Decision:** Use a real PostgreSQL database (dedicated or Testcontainers). Do not mock business rules in memory.

### Data isolation between tests

Pick one strategy and apply consistently:

- **Transaction rollback** per test (wrap in transaction, rollback in teardown)
- **Respawn** (reset tables between tests while keeping schema)
- **Recreate database** per test collection (slower, simplest)

Each test should start from a known state: seeded users exist; tickets are created via API helpers for the test.

### Test helpers (implementation phase)

| Helper | Purpose |
|--------|---------|
| `CreateTicketAsync(TicketStatus status)` | POST ticket, optionally PATCH to target status |
| `PatchStatusAsync(int id, string targetStatus)` | `PATCH /api/tickets/{id}/status` |
| `GetTicketAsync(int id)` | `GET /api/tickets/{id}` — verify persistence and `validNextStatuses` |
| `AssertValidTransition(from, to)` | Setup ticket in `from`, PATCH to `to`, assert 200 + DB state |
| `AssertInvalidTransition(from, to)` | Setup ticket in `from`, PATCH to `to`, assert 400 + unchanged DB |

### Assertions

**Valid transition (200):**

- HTTP `200`
- Response body `status` equals target
- `validNextStatuses` refreshed per current state (see matrix below)
- `updatedAt` is updated (≥ previous value)
- Re-fetch via `GET /api/tickets/{id}` confirms persisted status

**Invalid transition (400):**

- HTTP `400`
- Body: `{ "error": "Cannot transition from {current} to {target}", "code": "INVALID_TRANSITION" }`
- Ticket status in database **unchanged**

**Same-state transitions are NOT idempotent** — e.g. Open → Open returns `400` with `INVALID_TRANSITION`, not `200`.

---

## 3. Status state machine — complete transition matrix

Enforced exclusively via `PATCH /api/tickets/{id}/status`. Terminal states (`Closed`, `Cancelled`) have no outgoing transitions.

```
Open        → InProgress | Cancelled
InProgress  → Resolved   | Cancelled
Resolved    → Closed
```

### 3.1 Primary matrix — all 25 from×to pairs

| # | From | To | Valid? | Expected HTTP | Error code | Category | Test priority |
|---|------|-----|--------|---------------|------------|----------|---------------|
| 1 | Open | InProgress | Yes | 200 | — | Valid | **Mandatory** |
| 2 | Open | Cancelled | Yes | 200 | — | Valid | **Mandatory** |
| 3 | InProgress | Resolved | Yes | 200 | — | Valid | **Mandatory** |
| 4 | InProgress | Cancelled | Yes | 200 | — | Valid | **Mandatory** |
| 5 | Resolved | Closed | Yes | 200 | — | Valid | **Mandatory** |
| 6 | Open | Open | No | 400 | INVALID_TRANSITION | Same-state | Recommended |
| 7 | Open | Resolved | No | 400 | INVALID_TRANSITION | Skip | **Mandatory** |
| 8 | Open | Closed | No | 400 | INVALID_TRANSITION | Skip | **Mandatory** |
| 9 | InProgress | Open | No | 400 | INVALID_TRANSITION | Backward | Recommended |
| 10 | InProgress | InProgress | No | 400 | INVALID_TRANSITION | Same-state | Recommended |
| 11 | InProgress | Closed | No | 400 | INVALID_TRANSITION | Skip | **Mandatory** |
| 12 | Resolved | Open | No | 400 | INVALID_TRANSITION | Backward / no reopen | Recommended |
| 13 | Resolved | InProgress | No | 400 | INVALID_TRANSITION | Backward / no reopen | Recommended |
| 14 | Resolved | Resolved | No | 400 | INVALID_TRANSITION | Same-state | Recommended |
| 15 | Resolved | Cancelled | No | 400 | INVALID_TRANSITION | Post-resolve cancel | **Mandatory** |
| 16 | Closed | Open | No | 400 | INVALID_TRANSITION | Terminal | **Mandatory** |
| 17 | Closed | InProgress | No | 400 | INVALID_TRANSITION | Terminal | Recommended |
| 18 | Closed | Resolved | No | 400 | INVALID_TRANSITION | Terminal | Recommended |
| 19 | Closed | Closed | No | 400 | INVALID_TRANSITION | Terminal / same-state | Recommended |
| 20 | Closed | Cancelled | No | 400 | INVALID_TRANSITION | Terminal | Recommended |
| 21 | Cancelled | Open | No | 400 | INVALID_TRANSITION | Terminal | Recommended |
| 22 | Cancelled | InProgress | No | 400 | INVALID_TRANSITION | Terminal | **Mandatory** |
| 23 | Cancelled | Resolved | No | 400 | INVALID_TRANSITION | Terminal | Recommended |
| 24 | Cancelled | Closed | No | 400 | INVALID_TRANSITION | Terminal | Recommended |
| 25 | Cancelled | Cancelled | No | 400 | INVALID_TRANSITION | Terminal / same-state | Recommended |

**Summary:** 5 valid, 20 invalid (including 5 same-state no-ops).

### 3.2 Implementation guidance

| Approach | When |
|----------|------|
| **Minimum (prompt)** | 5 explicit valid tests + ≥4 explicit invalid tests (rows marked **Mandatory** above) |
| **Recommended** | Parameterized `[Theory]` over all 20 invalid pairs + 5 valid pairs — one test method per category |
| **Stretch** | Unit tests on `StatusTransitionService` for full 25-pair matrix without HTTP |

Invalid transition categories for readable test names:

- **Same-state** — e.g. Open → Open
- **Skip** — e.g. Open → Closed, Open → Resolved, InProgress → Closed
- **Backward** — e.g. InProgress → Open
- **Post-resolve cancel** — Resolved → Cancelled
- **Terminal** — any transition from Closed or Cancelled

### 3.3 `validNextStatuses` smoke tests (GET detail)

After a successful transition (or on create), `GET /api/tickets/{id}` should return:

| Current status | Expected `validNextStatuses` |
|----------------|------------------------------|
| Open | `["InProgress", "Cancelled"]` |
| InProgress | `["Resolved", "Cancelled"]` |
| Resolved | `["Closed"]` |
| Closed | `[]` |
| Cancelled | `[]` |

One smoke test per status is sufficient; not a separate test per transition.

---

## 4. Status endpoint isolation tests

Separate from the transition matrix — these verify that status cannot be mutated outside `PATCH /api/tickets/{id}/status`.

| Test | Action | Expected |
|------|--------|----------|
| Status on create | `POST /api/tickets` with `status` in body | `400` — `"Status cannot be set on create. Use PATCH /api/tickets/{id}/status."` |
| Status on update | `PUT /api/tickets/{id}` with `status` in body | `400` — `"Status cannot be updated via PUT. Use PATCH /api/tickets/{id}/status."` |
| Invalid enum on PATCH | `PATCH /api/tickets/{id}/status` with `{ "status": "Urgent" }` | `400` — `"Invalid status value: Urgent"` (no `INVALID_TRANSITION` code) |
| PATCH on missing ticket | `PATCH /api/tickets/99999/status` | `404` — `"Ticket not found"` |

---

## 5. Additional recommended integration tests

### 5.1 Validation (400)

| Test | Endpoint | Setup / input | Expected |
|------|----------|---------------|----------|
| Missing title | POST `/api/tickets` | Omit `title` | `400` — `"Title is required"` |
| Blank title | POST `/api/tickets` | `title: "   "` | `400` — trim then reject |
| Title too long | POST `/api/tickets` | `title` > 200 chars | `400` |
| Description too long | POST `/api/tickets` | `description` > 2000 chars | `400` |
| Invalid priority | POST `/api/tickets` | `priority: "Urgent"` | `400` |
| Non-existent createdBy | POST `/api/tickets` | `createdBy: 99999` | `400` — user does not exist |
| Non-existent assignedTo | POST `/api/tickets` | `assignedTo: 99999` | `400` |
| Blank comment message | POST `/api/tickets/{id}/comments` | `message: "   "` | `400` |
| Comment too long | POST comment | `message` > 1000 chars | `400` |
| Non-existent comment author | POST comment | `createdBy: 99999` | `400` |
| Invalid status filter | GET `/api/tickets?status=Urgent` | — | `400` — `"Invalid status value: Urgent"` |

### 5.2 Not found (404)

| Test | Endpoint | Expected |
|------|----------|----------|
| Get missing ticket | GET `/api/tickets/99999` | `404` — `"Ticket not found"` |
| Update missing ticket | PUT `/api/tickets/99999` | `404` |
| Status change missing ticket | PATCH `/api/tickets/99999/status` | `404` |
| Comment on missing ticket | POST `/api/tickets/99999/comments` | `404` |

### 5.3 Happy-path smoke (200 / 201)

| Test | Endpoint | Expected |
|------|----------|----------|
| List users | GET `/api/users` | `200`, seeded users returned |
| Create ticket | POST `/api/tickets` | `201`, `status: "Open"`, `validNextStatuses` populated |
| List tickets | GET `/api/tickets` | `200`, array of tickets |
| Search + filter | GET `/api/tickets?search=reset&status=Open` | `200`, filtered results |
| No matches | GET `/api/tickets?search=nonexistent` | `200`, `[]` (not an error) |
| Update fields on Closed ticket | PUT `/api/tickets/{id}` (ticket Closed) | `200`, fields updated, status still Closed |
| Comment on Closed ticket | POST comment on Closed ticket | `201` |
| Comment on Cancelled ticket | POST comment on Cancelled ticket | `201` |
| Comment order | GET detail after multiple comments | Comments oldest first |

---

## 6. Edge cases (reference)

Cross-reference `requirements-analysis.md` edge-case table. Key behaviors to cover in integration tests:

| # | Scenario | Covered by |
|---|----------|------------|
| 1–3 | Invalid / same-state / terminal transitions | State machine matrix (§3) |
| 4 | Status via PUT | Status isolation (§4) |
| 5–8 | Validation failures | Validation tests (§5.1) |
| 9 | Null assignee | Create/update with omitted `assignedTo` — accepted |
| 10–11 | Missing ticket | 404 tests (§5.2) |
| 12–14 | Comment validation | Validation tests (§5.1) |
| 15–16 | Empty search results | Happy-path smoke (§5.3) |
| 17 | Invalid status query | Validation (§5.1) |
| 18 | Concurrent updates | **Not tested in Core** — last-write-wins is non-deterministic |
| 19–20 | Edit/comment on Closed/Cancelled | Happy-path smoke (§5.3) |

---

## 7. Unit tests (optional Stretch)

Not required for Core. If added:

| Target | Coverage |
|--------|----------|
| `StatusTransitionService` | All 5 valid + all 20 invalid transition pairs; `GetValidNextStatuses` for each state |
| DTO validation | FluentValidation or data-annotation rules in isolation |

Unit tests complement but do not replace integration tests for AC-11.

---

## 8. Component and E2E tests (out of Core)

| Area | Reason |
|------|--------|
| Status dropdown shows only valid next states | UI concern; backend `validNextStatuses` is tested via API |
| Error toast on invalid transition | Manual demo sufficient |
| Playwright full flows | Not required; integration tests prove server rules |

---

## 9. What is NOT in scope for Core (and why)

| Area | Why out of scope |
|------|------------------|
| Authentication / JWT / role-based access | Explicitly excluded in Core; no login flow to test |
| User CRUD APIs and management UI | Users are seeded read-only; only `GET /api/users` |
| Ticket delete | Not in assessment spec; `Cancelled` is the abandon path |
| Pagination, sorting | Stretch features only |
| Swagger, Docker, CI pipelines | Stretch / operational concerns |
| Playwright / full UI E2E | Manual demo; backend integration tests prove business rules |
| Performance / load testing | Not required for one-week assessment |
| Comment edit or delete | Append-only in Core |
| Optimistic concurrency | Last-write-wins by design; concurrent test is flaky |
| Full 20 invalid transitions as separate non-parameterized tests | Parameterized `[Theory]` is preferred; runtime must still reject all 20 |

---

## 10. How to run and evidence

```bash
cd tests
dotnet test
```

At implementation time:

1. Create `tests/SupportTicket.Api.Tests/` with `WebApplicationFactory` setup
2. Configure test database connection (document in README and `.env.example`)
3. Run `dotnet test` and capture output in `test-results.md`

### Expected test counts (guidance)

| Category | Minimum | Recommended |
|----------|---------|-------------|
| Valid transitions | 5 | 5 (parameterized or explicit) |
| Invalid transitions | 4 | 20 (parameterized) |
| Status isolation | 2 | 4 |
| Validation | 0 | 8–10 |
| 404 | 0 | 4 |
| Happy-path smoke | 0 | 5–8 |

---

## Related documents

| Document | Content |
|----------|---------|
| `requirements-analysis.md` | State machine rules, edge cases, Core boundaries |
| `api-contract.md` | Endpoint shapes, error responses, transition examples |
| `acceptance-criteria.md` | AC-6, AC-11 testing requirements |
| `design-notes.md` | `StatusTransitionService`, architecture |
| `test-results.md` | Latest `dotnet test` output (implementation phase) |
