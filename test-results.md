# Test Results

## Run Information

| Field | Value |
|-------|-------|
| Date | 2026-07-23 |
| Command | `dotnet test tests/SupportTicket.Api.Tests/SupportTicket.Api.Tests.csproj` |
| Environment | local, Test DB: PostgreSQL `ticketdb_test` |
| Total tests | 70 |
| Passed | 70 |
| Failed | 0 |

## State Machine Integration Tests

| Test | Result | Notes |
|------|--------|-------|
| Open → InProgress (valid) | PASS | 200, status persisted |
| InProgress → Resolved (valid) | PASS | 200, status persisted |
| Resolved → Closed (valid) | PASS | 200, status persisted |
| Open → Cancelled (valid) | PASS | 200, status persisted |
| InProgress → Cancelled (valid) | PASS | 200, status persisted |
| Open → Closed (invalid) | PASS | 400, INVALID_TRANSITION |
| Open → Resolved (invalid) | PASS | 400, INVALID_TRANSITION |
| Closed → Open (invalid) | PASS | 400, INVALID_TRANSITION |
| Cancelled → InProgress (invalid) | PASS | 400, INVALID_TRANSITION |
| Resolved → Open (invalid) | PASS | 400, INVALID_TRANSITION |

## Other Tests

| Test | Result | Notes |
|------|--------|-------|
| Create ticket — empty title | PASS | 400, "Title is required" |
| Create ticket — invalid createdBy | PASS | 400, "User with id 99999 does not exist" |
| Get ticket — not found | PASS | 404, "Ticket not found" |
| Add comment — ticket not found | PASS | 404, "Ticket not found" |
| Add comment — empty message | PASS | 400, "Message is required" |

## Sample Output

```
Passed!  - Failed:     0, Passed:    70, Skipped:     0, Total:    70, Duration: 3 s - SupportTicket.Api.Tests.dll (net8.0)
```

## Failures and Fixes

| Failure | Root Cause | Fix | Re-run |
|---------|------------|-----|--------|
| — | — | — | — |
