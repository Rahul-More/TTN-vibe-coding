# Test Strategy

## Test Scope

**Core mandatory tier:** Integration tests proving the ticket status state machine.

Stretch (optional): unit tests for services, component tests for UI, edge-case tests.

## Unit Tests

_Not required for Core. Optional Stretch:_
- `StatusTransitionService` — all valid/invalid transition pairs
- Validation logic on DTOs

## Component Tests

_Not required for Core. Optional Stretch:_
- Status dropdown renders only valid next states
- Error toast on invalid transition

## API / Integration Tests

**Required for Core** — use `WebApplicationFactory` + test database.

| Test Case | Setup | Action | Expected |
|-----------|-------|--------|----------|
| Valid: Open → InProgress | Ticket with status Open | PATCH status InProgress | 200, status updated |
| Valid: InProgress → Resolved | Ticket InProgress | PATCH status Resolved | 200 |
| Valid: Resolved → Closed | Ticket Resolved | PATCH status Closed | 200 |
| Valid: Open → Cancelled | Ticket Open | PATCH status Cancelled | 200 |
| Valid: InProgress → Cancelled | Ticket InProgress | PATCH status Cancelled | 200 |
| Invalid: Open → Closed | Ticket Open | PATCH status Closed | 400, INVALID_TRANSITION |
| Invalid: Open → Resolved | Ticket Open | PATCH status Resolved | 400 |
| Invalid: Closed → Open | Ticket Closed | PATCH status Open | 400 |
| Invalid: Cancelled → InProgress | Ticket Cancelled | PATCH status InProgress | 400 |

Additional integration tests (recommended):
- Create ticket with missing title → 400
- Get non-existent ticket → 404
- Add comment to non-existent ticket → 404

## Edge Case Tests

| Case | Expected |
|------|----------|
| Double transition to same status | 400 or idempotent _(document choice)_ |
| Update ticket fields on Closed ticket | Allowed _(fields only, not status)_ |

## Tests Not Covered (and why)

| Area | Reason |
|------|--------|
| Full UI E2E (Playwright) | Out of Core scope; manual demo sufficient |
| Performance/load testing | Not required for assessment |
| Auth flows | Optional Stretch only |

## How to Run

```bash
cd tests
dotnet test
```

Document actual results in `test-results.md`.
