---
name: core-test-coverage-review
overview: Map all 11 Core acceptance criteria to existing tests or manual steps, close the Core-only automated gaps (status isolation on POST/PUT, invalid enum/query validation, search/filter), regenerate an AC-coverage summary table in test-results.md, and fill in the Prompt 3 response log in ai-prompts/testing.md.
todos:
  - id: mapping
    content: Produce AC-1..AC-11 -> test/manual mapping and add Acceptance Criteria Coverage table to test-results.md
    status: completed
  - id: gap-tests
    content: "Add Core-only integration tests: status isolation (POST/PUT/PATCH enum), invalid priority, invalid status query, search/filter + empty [], and same-state transition row"
    status: completed
  - id: helpers
    content: Add any missing raw HTTP helpers (PutTicketRawAsync, list raw helper) to TicketApiHelpers
    status: completed
  - id: results
    content: Run dotnet test, refresh totals and per-test tables in test-results.md
    status: completed
  - id: response-log
    content: Fill Prompt 3 response log in ai-prompts/testing.md
    status: completed
isProject: false
---

## Core Test Coverage Review

### 1. Acceptance-criterion coverage mapping

Current state per AC (existing tests in `tests/SupportTicket.Api.Tests/`):

- AC-1 Create ticket: PARTIAL automated — empty title + invalid `createdBy` (`ValidationErrorIntegrationTests`); status-cannot-be-set-on-create NOT tested (behavior exists in `CreateTicketRequestValidator`); UI form = manual
- AC-2 View all tickets (`GET /api/tickets`): NO automated test = manual demo
- AC-3 Detail + comments oldest-first: NO automated test for comment ordering = manual demo
- AC-4 Update/reassign; status-on-PUT rejected; edit Closed/Cancelled: NO automated test; status-on-PUT rejection exists in `UpdateTicketRequestValidator` but untested
- AC-5 Add comments happy path + on Closed/Cancelled: PARTIAL — only 400/404 negatives tested; happy-path create = manual
- AC-6 Valid transitions only: STRONG — 5 valid + 5 invalid integration + full 25-pair unit + `validNextStatuses` asserted; UI dropdown = manual
- AC-7 Search + status filter + empty `[]`: NO automated test
- AC-8 Data survives restart: MANUAL only (documented restart step)
- AC-9 Backend validation: PARTIAL — title/createdBy/message covered; max-length, invalid priority, invalid status query, non-existent assignedTo untested
- AC-10 No secrets in repo: MANUAL repo/`.env.example` inspection
- AC-11 State-machine integration tests: FULLY covered (`StatusTransitionIntegrationTests` + documented `dotnet test`)

### 2. Core coverage gaps (backend enforced, currently untested)

All confirmed present in code, so new tests will pass:

- Status in `POST /api/tickets` body -> 400 `"Status cannot be set on create..."` (AC-1)
- Status in `PUT /api/tickets/{id}` body -> 400 `"Status cannot be updated via PUT..."` (AC-4)
- `GET /api/tickets?status=Urgent` -> 400 invalid status value (AC-7 / AC-9)
- `POST /api/tickets` with `priority: "Urgent"` -> 400 (AC-9)
- Same-state transition (e.g. Open -> Open) covered in unit but NOT in integration matrix (AC-6 edge case)

Manual-only ACs (no automated test suggested): AC-2, AC-3 comment order, AC-8 restart, AC-10 secrets, and all UI surfacing.

### 3. Suggested additional Core tests (implement)

Add to `tests/SupportTicket.Api.Tests/Integration/` reusing `CustomWebApplicationFactory` + `TicketApiHelpers` (may need a small `PutTicketRawAsync` helper and a raw list helper):

- `StatusIsolationIntegrationTests`: status-on-POST 400, status-on-PUT 400, invalid enum on PATCH 400 (no `INVALID_TRANSITION` code)
- Extend `ValidationErrorIntegrationTests`: invalid priority 400, invalid `status` query param 400
- `SearchFilterIntegrationTests`: seed tickets, assert `search`+`status` filtering and empty result returns 200 `[]`
- Add same-state row (Open -> Open) to `StatusTransitionIntegrationTests.InvalidTransitions`

Scope note: these all map to Core ACs. No Stretch tests (no React component/E2E, no concurrency) will be added.

### 4. Regenerate test-results.md summary table

Update [test-results.md](test-results.md) to add an "Acceptance Criteria Coverage" section: one row per AC-1..AC-11 with columns Criterion / Covered By (test name or "Manual") / Status (fill-in) / Notes, plus keep the existing per-test result tables and refresh totals after the new tests run.

### 5. Update Prompt 3 response log

Fill the empty response-log table under "Prompt 3" in [ai-prompts/testing.md](ai-prompts/testing.md) (Date, AI response summary, Accepted, Changed, Rejected, Why, Test run result) describing this coverage review and the added Core tests.

### Run + verify

Run `dotnet test tests/SupportTicket.Api.Tests/SupportTicket.Api.Tests.csproj`, capture the new pass count, and record it in both `test-results.md` and the Prompt 3 response log.