# Test Results

## Run Information

| Field | Value |
|-------|-------|
| Date | 2026-07-23 |
| Command | `dotnet test tests/SupportTicket.Api.Tests/SupportTicket.Api.Tests.csproj` |
| Environment | local, Test DB: PostgreSQL `ticketdb_test` |
| Total tests | 78 |
| Passed | 78 |
| Failed | 0 |

## Acceptance Criteria Coverage

| Criterion | Covered By | Status | Notes |
|-----------|------------|--------|-------|
| **AC-1** Create ticket via UI | `PostTicket_EmptyTitle_Returns400`, `PostTicket_InvalidCreatedBy_Returns400`, `PostTicket_InvalidPriority_Returns400`, `PostTicket_WithStatusInBody_Returns400` + **Manual** UI form demo | | API validation automated; UI form + default `Open` status require manual demo |
| **AC-2** View all tickets | **Manual** UI list demo | | `GET /api/tickets` happy path not automated; list loads from real DB verified in demo |
| **AC-3** Ticket detail view | `GetTicket_NonExistentId_Returns404` + **Manual** detail page demo | | Comment thread oldest-first requires manual demo |
| **AC-4** Update fields and reassign | `PutTicket_WithStatusInBody_Returns400` + **Manual** edit/reassign demo | | Field update persistence and Closed/Cancelled edit require manual demo |
| **AC-5** Add comments | `PostComment_EmptyMessage_Returns400`, `PostComment_NonExistentTicket_Returns404` + **Manual** comment thread demo | | Happy-path comment create on Open/Closed/Cancelled requires manual demo |
| **AC-6** Valid transitions only | `StatusTransitionIntegrationTests` (6 invalid + 5 valid), `StatusTransitionServiceTests` (full 25-pair matrix + `validNextStatuses`) + **Manual** UI status dropdown | | Backend fully proven; UI shows only valid next statuses = manual |
| **AC-7** Search and status filter | `GetTickets_WithSearchAndStatusFilter_ReturnsMatchingTickets`, `GetTickets_WithNoMatches_Returns200AndEmptyArray`, `GetTickets_InvalidStatusQueryParam_Returns400` + **Manual** empty-state UI | | API search/filter/empty `[]` automated; friendly UI empty state = manual |
| **AC-8** Data survives restart | **Manual** restart verification | | Stop/restart app + DB; confirm tickets/comments/users persist |
| **AC-9** Backend validation | `ValidationErrorIntegrationTests`, `StatusIsolationIntegrationTests`, `PatchStatus_InvalidStatusEnum_Returns400WithoutInvalidTransitionCode` | | Core validation rules covered; max-length and non-existent `assignedTo` not automated |
| **AC-10** No secrets in repo | **Manual** repo inspection | | Verify no connection strings/keys in code; `.env.example` present |
| **AC-11** State-machine integration tests | `StatusTransitionIntegrationTests` + `dotnet test` documented | | All 5 valid + representative invalid transitions via `WebApplicationFactory` + Postgres |

## State Machine Integration Tests

| Test | Result | Notes |
|------|--------|-------|
| Open → InProgress (valid) | PASS | 200, status persisted, `validNextStatuses` updated |
| InProgress → Resolved (valid) | PASS | 200, status persisted |
| Resolved → Closed (valid) | PASS | 200, status persisted |
| Open → Cancelled (valid) | PASS | 200, status persisted |
| InProgress → Cancelled (valid) | PASS | 200, status persisted |
| Open → Open (invalid, same-state) | PASS | 400, INVALID_TRANSITION |
| Open → Closed (invalid) | PASS | 400, INVALID_TRANSITION |
| Open → Resolved (invalid) | PASS | 400, INVALID_TRANSITION |
| Closed → Open (invalid) | PASS | 400, INVALID_TRANSITION |
| Cancelled → InProgress (invalid) | PASS | 400, INVALID_TRANSITION |
| Resolved → Open (invalid) | PASS | 400, INVALID_TRANSITION |

## Validation and Error Integration Tests

| Test | Result | Notes |
|------|--------|-------|
| Create ticket — empty title | PASS | 400, "Title is required" |
| Create ticket — invalid createdBy | PASS | 400, "User with id 99999 does not exist" |
| Create ticket — invalid priority | PASS | 400, "Invalid priority value: Urgent" |
| Get ticket — not found | PASS | 404, "Ticket not found" |
| Add comment — ticket not found | PASS | 404, "Ticket not found" |
| Add comment — empty message | PASS | 400, "Message is required" |
| List tickets — invalid status query | PASS | 400, "Invalid status value: Urgent" |

## Status Isolation Integration Tests

| Test | Result | Notes |
|------|--------|-------|
| Create ticket — status in body | PASS | 400, status cannot be set on create |
| Update ticket — status in body | PASS | 400, status cannot be updated via PUT |
| PATCH status — invalid enum | PASS | 400, no INVALID_TRANSITION code |

## Search and Filter Integration Tests

| Test | Result | Notes |
|------|--------|-------|
| Combined search + status filter | PASS | 200, single matching ticket |
| No matches | PASS | 200, empty array `[]` |

## Unit Tests (StatusTransitionService)

| Test | Result | Notes |
|------|--------|-------|
| Full 25-pair transition matrix | PASS | `IsValidTransition` + `ValidateTransition` |
| `GetValidNextStatuses` per state | PASS | All 5 statuses |

## Sample Output

```
Passed!  - Failed:     0, Passed:    78, Skipped:     0, Total:    78, Duration: 2 s - SupportTicket.Api.Tests.dll (net8.0)
```

## Failures and Fixes

| Failure | Root Cause | Fix | Re-run |
|---------|------------|-----|--------|
| — | — | — | — |
