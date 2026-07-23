---
name: Apply review fixes
overview: Apply all accepted Prompt 1/2 code-review findings (#1–#7, F1–F3) with minimal Core-scope edits, run existing API tests, then update Prompt 3 tracking docs.
todos:
  - id: backend-security-config
    content: "#1–#2: Remove appsettings ConnectionStrings; harden AppDbContextFactory"
    status: completed
  - id: backend-enum-json-swagger
    content: "#3–#5: Enum.IsDefined, 500 camelCase JSON, remove Swagger"
    status: completed
  - id: backend-services
    content: "#6–#7: Defensive guards + shared ticket mapper"
    status: completed
  - id: frontend-a11y-error
    content: "F1–F3: Search label, skip table on error, Priority aria-labels"
    status: completed
  - id: docs-and-tests
    content: Update code-review.md / notes / review-fixes.md; run dotnet test
    status: completed
isProject: false
---

# Apply Code Review Fixes (Prompt 3)

Accepted scope: backend **#1–#7** and frontend **F1–F3**. Swagger will be **removed** (not gated) to keep Core free of Stretch surface.

## Backend

### #1 — Remove committed connection strings
- Delete the `ConnectionStrings` blocks from [`src/SupportTicket.Api/appsettings.json`](src/SupportTicket.Api/appsettings.json) and [`src/SupportTicket.Api/appsettings.Development.json`](src/SupportTicket.Api/appsettings.Development.json).
- Leave runtime read as-is: `GetConnectionString("DefaultConnection")` (env `ConnectionStrings__DefaultConnection` already documented in [`.env.example`](.env.example)).

### #2 — `AppDbContextFactory` no password fallback
In [`src/SupportTicket.Api/AppDbContextFactory.cs`](src/SupportTicket.Api/AppDbContextFactory.cs): require `ConnectionStrings__DefaultConnection`; if missing, throw a clear exception (no literal connection string).

### #3 — Strict enum parse
In [`src/SupportTicket.Api/Helpers/EnumParsing.cs`](src/SupportTicket.Api/Helpers/EnumParsing.cs): after `Enum.TryParse`, require `Enum.IsDefined` so numeric/out-of-range values (`"5"`, `"9"`) fail and existing invalid-status/priority messages still apply via callers/validators.

### #4 — CamelCase 500 body
In [`src/SupportTicket.Api/Middleware/ExceptionHandlingMiddleware.cs`](src/SupportTicket.Api/Middleware/ExceptionHandlingMiddleware.cs): serialize with `PropertyNamingPolicy = JsonNamingPolicy.CamelCase` so the body is `{"error":"..."}`.

### #5 — Remove Swagger
In [`src/SupportTicket.Api/Program.cs`](src/SupportTicket.Api/Program.cs): remove `AddEndpointsApiExplorer` / `AddSwaggerGen` registration and the Development `UseSwagger` / `UseSwaggerUI` block. Remove the Swashbuckle package reference from the API `.csproj` if nothing else needs it.

### #6 — Defensive service guards
In [`src/SupportTicket.Api/Services/TicketService.cs`](src/SupportTicket.Api/Services/TicketService.cs): honor `TryParsePriority` in `CreateAsync` / `UpdateAsync`; on failure return `ServiceResult` fail with `InvalidPriorityMessage`.
In [`src/SupportTicket.Api/Services/CommentService.cs`](src/SupportTicket.Api/Services/CommentService.cs): if `createdBy` is null, return fail instead of `createdBy!.Name`.

### #7 — Deduplicate ticket mapping
In `TicketService.cs`: extract a private helper for shared list fields; `MapToListItem` / `MapToDetail` reuse it (detail still adds `ValidNextStatuses` + `Comments`).

## Frontend

### F1 — Search a11y
[`src/SupportTicket.Web/src/pages/TicketListPage.tsx`](src/SupportTicket.Web/src/pages/TicketListPage.tsx): add `label="Search"` on the search `TextField` (matches Status `label` pattern).

### F2 — No empty table under error
Same file: when `error` is set and not loading, render only the error banner (skip `EmptyState` and table).

### F3 — Priority group a11y
[`src/SupportTicket.Web/src/pages/CreateTicketPage.tsx`](src/SupportTicket.Web/src/pages/CreateTicketPage.tsx): add `aria-label="Priority"` on the `ToggleButtonGroup`, and per-button `aria-label={`${p} priority`}` to match [`TicketDetailPage.tsx`](src/SupportTicket.Web/src/pages/TicketDetailPage.tsx).

## Docs (Prompt 3 tracking)

- Fill **Prompt 3 Response Log** in [`ai-prompts/code-review.md`](ai-prompts/code-review.md) (date, fixes applied, files changed, tests pass).
- Update [`code-review-notes.md`](code-review-notes.md): mark Actions **Accept**; fill **Changes Made** / **Suggestions Rejected** (none rejected).
- Fill [`review-fixes.md`](review-fixes.md) with one row per applied fix.

## Validation

```bash
dotnet test c:\Work\Github\TTN-vibe-coding\tests\SupportTicket.Api.Tests\SupportTicket.Api.Tests.csproj
```

No frontend automated suite expected for F1–F3; note that in the Prompt 3 log. Do not add Stretch features.