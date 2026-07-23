---
name: Validation Error Tests
overview: Fix FluentValidation async auto-validation so POST endpoints work, then add five validation/404 integration tests reusing the existing WebApplicationFactory, update test-results.md, and fill the Prompt 2 response log in testing.md.
todos:
  - id: fix-async-validation
    content: Replace FluentValidation sync auto-validation with async action filter in API
    status: completed
  - id: extend-helpers
    content: Add POST ticket/comment and raw GET helpers to TicketApiHelpers
    status: completed
  - id: write-validation-tests
    content: Add ValidationErrorIntegrationTests covering the 5 required cases
    status: completed
  - id: run-dotnet-test
    content: Run dotnet test until green against ticketdb_test
    status: completed
  - id: update-docs
    content: Update test-results.md and Prompt 2 response log in ai-prompts/testing.md
    status: completed
isProject: false
---

# Validation & Error Integration Tests

## Prerequisite: unblock POST (FluentValidation async)

Prompt 1 seeded via EF because `AddFluentValidationAutoValidation()` invokes validators **synchronously**, and `MustAsync` user-existence rules throw on every POST/PUT/comment. The five new tests all hit those paths, so this must be fixed first.

**Change in** [`src/SupportTicket.Api/Program.cs`](src/SupportTicket.Api/Program.cs):

- Remove `AddFluentValidationAutoValidation()`
- Keep `AddValidatorsFromAssemblyContaining<...>()` and the existing `InvalidModelStateResponseFactory` (`{ "error": "..." }`)
- Register a small `IAsyncActionFilter` (e.g. [`src/SupportTicket.Api/Filters/FluentValidationAsyncActionFilter.cs`](src/SupportTicket.Api/Filters/FluentValidationAsyncActionFilter.cs)) that:
  1. For each action argument, resolves `IValidator<T>` if registered
  2. Calls `ValidateAsync`
  3. On failure, adds the first error into `ModelState` and returns `BadRequestObjectResult(new ErrorResponse(...))` matching the current factory shape

No new packages. Validators stay unchanged. Existing status PATCH tests keep working (`ChangeStatusRequestValidator` is sync-only).

## Tests to add

New file: [`tests/SupportTicket.Api.Tests/Integration/ValidationErrorIntegrationTests.cs`](tests/SupportTicket.Api.Tests/Integration/ValidationErrorIntegrationTests.cs)

Reuse `[Collection("Integration")]` + `CustomWebApplicationFactory` / `factory.Client` / `factory.Services` (same as [`StatusTransitionIntegrationTests.cs`](tests/SupportTicket.Api.Tests/Integration/StatusTransitionIntegrationTests.cs)).

| # | Test | Call | Assert |
|---|------|------|--------|
| 1 | Empty title | `POST /api/tickets` with `title: ""`, valid `priority`/`createdBy: 1` | **400**, `error == "Title is required"` |
| 2 | Invalid createdBy | `POST /api/tickets` with valid title, `createdBy: 99999` | **400**, `error == "User with id 99999 does not exist"` |
| 3 | Missing ticket GET | `GET /api/tickets/999999` | **404**, `error == "Ticket not found"` |
| 4 | Comment missing ticket | `POST /api/tickets/999999/comments` with `message` + `createdBy: 1` | **404**, `error == "Ticket not found"` |
| 5 | Empty comment message | Seed ticket via existing helper, `POST .../comments` with `message: ""`, `createdBy: 1` | **400**, `error == "Message is required"` |

Error body assertions via existing `TicketApiHelpers.ReadErrorAsync` (no `code` field for these cases per [`api-contract.md`](api-contract.md)).

## Helper extensions

Extend [`TicketApiHelpers.cs`](tests/SupportTicket.Api.Tests/Helpers/TicketApiHelpers.cs) with thin HTTP helpers used by the new tests:

- `PostTicketRawAsync(client, body)` → `POST /api/tickets`
- `PostCommentRawAsync(client, ticketId, body)` → `POST /api/tickets/{id}/comments`
- `GetTicketRawAsync(client, ticketId)` → raw GET (for 404; existing `GetTicketAsync` calls `EnsureSuccessStatusCode`)

## Docs after green run

1. **[`test-results.md`](test-results.md)** — fill Run Information; keep/pass state-machine rows; expand **Other Tests** to all five cases above with PASS/notes; paste concise `dotnet test` sample output.
2. **[`ai-prompts/testing.md`](ai-prompts/testing.md)** Prompt 2 Response Log — Date `2026-07-23`, summary of filter fix + 5 tests, Accepted/Changed/Rejected/Why (reject InMemory / new deps), Test run result from `dotnet test`.

## Validation

Run `dotnet test` (requires local Postgres `ticketdb_test` as today). Expect prior 65 tests still green + 5 new = **70 passed**.
