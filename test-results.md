# Test Results

## Run Information

| Field | Value |
|-------|-------|
| Date | _[YYYY-MM-DD]_ |
| Command | `dotnet test` |
| Environment | _[e.g. local, Test DB: SQLite in-memory]_ |
| Total tests | _[N]_ |
| Passed | _[N]_ |
| Failed | _[N]_ |

## State Machine Integration Tests

| Test | Result | Notes |
|------|--------|-------|
| Open → InProgress (valid) | _[PASS/FAIL]_ | |
| InProgress → Resolved (valid) | _[PASS/FAIL]_ | |
| Resolved → Closed (valid) | _[PASS/FAIL]_ | |
| Open → Cancelled (valid) | _[PASS/FAIL]_ | |
| InProgress → Cancelled (valid) | _[PASS/FAIL]_ | |
| Open → Closed (invalid) | _[PASS/FAIL]_ | |
| Open → Resolved (invalid) | _[PASS/FAIL]_ | |
| Closed → Open (invalid) | _[PASS/FAIL]_ | |
| Cancelled → InProgress (invalid) | _[PASS/FAIL]_ | |

## Other Tests

| Test | Result | Notes |
|------|--------|-------|
| Create ticket — missing title | _[PASS/FAIL]_ | |
| Get ticket — not found | _[PASS/FAIL]_ | |
| Add comment — ticket not found | _[PASS/FAIL]_ | |

## Sample Output

```
_Paste dotnet test output here after running tests_
```

## Failures and Fixes

_If any test failed, describe the fix and re-run result._

| Failure | Root Cause | Fix | Re-run |
|---------|------------|-----|--------|
| _[example]_ | _[example]_ | _[example]_ | _[PASS]_ |
