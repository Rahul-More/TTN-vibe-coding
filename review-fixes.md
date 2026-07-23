# Review Fixes

Track concrete fixes applied after AI-assisted and self code review.

| # | Review Source | Issue | Fix Applied | File(s) | Verified |
|---|---------------|-------|-------------|---------|----------|
| 1 | AI review (Prompt 1 #1) | Committed connection strings in appsettings | Removed `ConnectionStrings` blocks; config via `ConnectionStrings__DefaultConnection` env var | `appsettings.json`, `appsettings.Development.json` | `dotnet test` (78/78) |
| 2 | AI review (Prompt 1 #2) | Hardcoded fallback in `AppDbContextFactory` | Throw `InvalidOperationException` when env var unset | `AppDbContextFactory.cs` | `dotnet test` |
| 3 | AI review (Prompt 1 #3) | Numeric/undefined enum strings accepted | Added `Enum.IsDefined` check after `TryParse` | `EnumParsing.cs` | `dotnet test` |
| 4 | AI review (Prompt 1 #4) | 500 error body PascalCase | Serialize with `JsonNamingPolicy.CamelCase` | `ExceptionHandlingMiddleware.cs` | `dotnet test` |
| 5 | AI review (Prompt 1 #5) | Swagger beyond Core scope | Removed Swagger registration, middleware, and Swashbuckle package | `Program.cs`, `SupportTicket.Api.csproj` | `dotnet test` |
| 6 | AI review (Prompt 1 #6) | Services ignore parse/null guards | `TryParsePriority` fail in create/update; null `createdBy` check | `TicketService.cs`, `CommentService.cs` | `dotnet test` |
| 7 | AI review (Prompt 1 #7) | Duplicated ticket field mapping | Extracted `MapListFields` shared by list/detail mappers | `TicketService.cs` | `dotnet test` |
| 8 | AI review (Prompt 2 F1) | Search field missing accessible label | Added `label="Search"` on search `TextField` | `TicketListPage.tsx` | Manual / visual |
| 9 | AI review (Prompt 2 F2) | Empty table under error banner | Skip table/empty state when `error` is set | `TicketListPage.tsx` | Manual / visual |
| 10 | AI review (Prompt 2 F3) | Priority toggle group missing aria-label | Added group `aria-label="Priority"` and per-button labels | `CreateTicketPage.tsx` | Manual / visual |

## Commit References

Point to commits where review fixes were applied (for submission form):

- _[commit hash]_ — _[description]_
