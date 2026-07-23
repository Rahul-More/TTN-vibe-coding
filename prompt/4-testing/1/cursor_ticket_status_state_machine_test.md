# Ticket status state machine tests
_Exported on 7/23/2026 at 22:32:07 GMT+5:30 from Cursor (3.12.30)_

---

**User**

@ai-prompts/testing.md:16-42 Write integration tests for the ticket status state machine.

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
@test-strategy.md @api-contract.md @requirements-analysis.md 

---

**Cursor**

> Auto routed to grok-4.5



I'll research the test strategy, API contract, and existing ticket status implementation so we can plan the integration tests accurately.

Next I'll read the strategy docs and key API/test files to lock the WebApplicationFactory and DB approach.

Checking DB config, seeding, and whether Docker/Postgres is already available for the preferred test setup.

I have enough context: the strategy prefers real PostgreSQL, and the API already targets local Postgres—I'll plan against that and skip EF InMemory.

---

**User**

Implement the plan as specified, it is attached for your reference. Do NOT edit the plan file itself.

To-do's from the plan have already been created. Do not create them again. Mark them as in_progress as you work, starting with the first one. Don't stop until you have completed all the to-dos.

---

**Cursor**

> Auto routed to composer-2.5



I'll implement the integration test infrastructure and status transition tests per the plan, starting with the test project package and factory setup.


Fixing the missing helper method and verifying the factory initializes correctly.


Build failed because the API process is locking the executable. I'll build without rebuilding the API reference and fix factory initialization.


Switching to lazy initialization to avoid the `IAsyncLifetime` conflict with `WebApplicationFactory`.


POST is failing due to async FluentValidation in the test host. I'll inspect the validator and how the app configures validation.


Seeding tickets via DbContext instead of POST to avoid the app's async FluentValidation issue while still exercising PATCH over HTTP.


Sequence isn't advanced after seed data inserts explicit IDs — I'll reset it and disable parallel integration tests.


All 65 tests passed. Marking the final todo complete.


Integration tests for the ticket status state machine are in place and all **65 tests pass** (`dotnet test`).

### What was added

**Infrastructure**
- [`tests/SupportTicket.Api.Tests/Infrastructure/CustomWebApplicationFactory.cs`](tests/SupportTicket.Api.Tests/Infrastructure/CustomWebApplicationFactory.cs) — `WebApplicationFactory<Program>` targeting `ticketdb_test`, with migrate/seed/reset on startup
- [`tests/SupportTicket.Api.Tests/Helpers/TicketApiHelpers.cs`](tests/SupportTicket.Api.Tests/Helpers/TicketApiHelpers.cs) — seeds tickets via EF, PATCH/GET via HTTP
- [`tests/SupportTicket.Api.Tests/Integration/IntegrationCollection.cs`](tests/SupportTicket.Api.Tests/Integration/IntegrationCollection.cs) — serializes integration tests to avoid DB races

**Tests**
- [`tests/SupportTicket.Api.Tests/Integration/StatusTransitionIntegrationTests.cs`](tests/SupportTicket.Api.Tests/Integration/StatusTransitionIntegrationTests.cs) — 5 valid + 5 invalid transitions using `[Theory]`

**Package**
- `Microsoft.AspNetCore.Mvc.Testing` 8.0.11 in the test project

### Test coverage

| Valid (200) | Invalid (400 + `INVALID_TRANSITION`) |
|---|---|
| Open → InProgress | Open → Closed |
| InProgress → Resolved | Open → Resolved |
| Resolved → Closed | Closed → Open |
| Open → Cancelled | Cancelled → InProgress |
| InProgress → Cancelled | Resolved → Open |

Each test seeds a ticket at the starting status, calls `PATCH /api/tickets/{id}/status`, and asserts status code, response body, and persisted state via GET.

### Notes
- Tests use a dedicated PostgreSQL DB (`ticketdb_test`); local Postgres must be running.
- Tickets are seeded via EF (not POST) because `CreateTicketRequestValidator` uses async rules incompatible with FluentValidation auto-validation — worth fixing separately for Prompt 2.
- If the API is running, build with `-p:BuildProjectReferences=false` to avoid file-lock errors.

