# AI Prompts — Testing Phase

> **How to use:** New chat per prompt. Run `dotnet test` after each prompt and log results in `test-results.md`.

---

## Prompt 1 — State Machine Integration Tests

**When:** After backend API is complete. New chat session.

**@ references:** `test-strategy.md`, `api-contract.md`, `requirements-analysis.md`

### Copy into chat:

```
Write integration tests for the ticket status state machine.

Read @test-strategy.md and @requirements-analysis.md.

Use WebApplicationFactory with an in-memory or test database.

Required tests:
VALID transitions (must return 200):
- Open → InProgress
- InProgress → Resolved
- Resolved → Closed
- Open → Cancelled
- InProgress → Cancelled

INVALID transitions (must return 400):
- Open → Closed
- Open → Resolved
- Closed → Open
- Cancelled → InProgress
- Resolved → Open

Each test should:
1. Seed a ticket with the starting status
2. Call PATCH /api/tickets/{id}/status
3. Assert status code and response body

Place tests in tests/ folder. Ensure dotnet test runs successfully.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | 2026-07-23 |
| **AI response summary** | Added WebApplicationFactory integration tests for 5 valid + 5 invalid status transitions against PostgreSQL `ticketdb_test`. Includes factory, helpers, and `[Theory]` tests under `tests/SupportTicket.Api.Tests/Integration/`. |
| **Accepted** | CustomWebApplicationFactory with dedicated Postgres test DB; StatusTransitionIntegrationTests covering all 10 required transitions; PATCH assertions for 200/`TicketDetailResponse` and 400/`INVALID_TRANSITION`; GET persistence checks. |
| **Changed** | Seeded tickets via EF DbContext instead of POST (async FluentValidation auto-validation crashes POST); reset Tickets identity sequence after seed; used `[Collection]` to disable parallel integration tests. |
| **Rejected** | EF InMemory (strategy prefers real Postgres); Testcontainers (no Docker compose in repo). |
| **Why** | Matches `test-strategy.md` AC for state-machine HTTP proof; POST seed blocked by existing FluentValidation async-rule issue in API. |
| **Test run result** | `dotnet test` — 65 passed, 0 failed (55 unit + 10 integration). |

---

## Prompt 2 — Validation & Error Integration Tests

**When:** After state machine tests pass. New chat session.

**@ references:** `test-strategy.md`, `api-contract.md`

### Copy into chat:

```
Add integration tests for validation and error handling.

Read @test-strategy.md and @api-contract.md.

Tests to add:
1. POST /api/tickets with empty title → 400
2. POST /api/tickets with invalid createdBy user id → 400
3. GET /api/tickets/{id} with non-existent id → 404
4. POST comment on non-existent ticket → 404
5. POST comment with empty message → 400

Use the same WebApplicationFactory setup as existing tests.
Update test-results.md with a summary table after running.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | 2026-07-23 |
| **AI response summary** | Replaced sync FluentValidation auto-validation with `FluentValidationAsyncActionFilter` so async `MustAsync` rules work on POST; added 5 validation/404 integration tests and HTTP helpers in `ValidationErrorIntegrationTests`. |
| **Accepted** | Async action filter for FluentValidation; `ValidationErrorIntegrationTests` for empty title, invalid createdBy, GET 404, comment 404, empty message; extended `TicketApiHelpers` with POST/GET raw helpers. |
| **Changed** | Removed `AddFluentValidationAutoValidation()` from `Program.cs`; added `Filters/FluentValidationAsyncActionFilter.cs`; updated `test-results.md` with all 70 passing tests. |
| **Rejected** | EF InMemory (strategy prefers Postgres); new NuGet packages (filter uses existing FluentValidation DI). |
| **Why** | POST endpoints require async validator support for user-existence checks; matches `test-strategy.md` §5.1/§5.2 and `api-contract.md` error envelopes. |
| **Test run result** | `dotnet test` — 70 passed, 0 failed (55 unit + 15 integration). |

---

## Prompt 3 — Validate Tests Against Acceptance Criteria

**When:** After all tests pass. New chat session.

**@ references:** `acceptance-criteria.md`, `test-results.md`, `test-strategy.md`

### Copy into chat:

```
Review my test coverage against the Core acceptance criteria.

Read @acceptance-criteria.md and @test-strategy.md.

1. Map each acceptance criterion to a test or manual verification step
2. Identify any gaps in test coverage
3. Suggest additional tests only if required for Core (not Stretch)
4. Generate a test-results.md summary table I can fill in

Do not add Stretch tests unless I explicitly ask.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | |
| **AI response summary** | |
| **Accepted** | |
| **Changed** | |
| **Rejected** | |
| **Why** | |
