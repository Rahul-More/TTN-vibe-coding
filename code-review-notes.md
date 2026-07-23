# Code Review Notes

## AI-Assisted Review Summary

**Prompt used:** `ai-prompts/code-review.md` → Prompt 1

**Date:** 2026-07-23

**Scope reviewed:** Backend (`src/SupportTicket.Api`) against `acceptance-criteria.md` and `api-contract.md`

### Review checklist (Prompt 1)

| # | Area | Verdict |
|---|------|---------|
| 1 | Status state machine — logic in ONE place only? | Pass — `StatusTransitionService` is sole enforcement point |
| 2 | Validation — required fields checked on backend? | Mostly pass — FluentValidation covers Core rules; numeric enum gap (#3) |
| 3 | Error handling — correct HTTP codes (400, 404, 500)? | Mostly pass — codes OK; 500 JSON casing off-contract (#4) |
| 4 | Security — hardcoded secrets or connection strings? | Fail — committed connection strings (#1, #2) |
| 5 | API contract compliance | Mostly pass — endpoints/DTOs match; enum + 500 deviations |
| 6 | Code quality | Pass with minor notes (#6, #7) |
| 7 | Over-engineering beyond Core | Swagger present — Stretch (#5) |

### AI Findings

| # | File | Finding | Severity | Suggested fix | Action |
|---|------|---------|----------|---------------|--------|
| 1 | `src/SupportTicket.Api/appsettings.json`, `appsettings.Development.json` | Git-tracked files contain full connection string with `Password=password`. Violates AC-10 (no connection strings committed; config via env vars). | High | Remove `ConnectionStrings` from committed appsettings; source from `ConnectionStrings__DefaultConnection` (already in `.env.example`); gitignore local overrides if needed. | Pending |
| 2 | `src/SupportTicket.Api/AppDbContextFactory.cs` (lines 13–14) | Hardcoded fallback connection string with password literal. | Med | Drop literal fallback; throw clear error if env var unset, or read config only. | Pending |
| 3 | `src/SupportTicket.Api/Helpers/EnumParsing.cs` (lines 10, 16) | `Enum.TryParse` accepts numeric strings (`status=5`, `priority=9`). List filter may return `200 []` instead of `400`; PATCH may return `INVALID_TRANSITION` instead of `"Invalid status value: ..."`. | Med | After parse, require `Enum.IsDefined(...)`, or reject digit-only inputs. | Pending |
| 4 | `src/SupportTicket.Api/Middleware/ExceptionHandlingMiddleware.cs` (line 33) | 500 body uses default `JsonSerializer` → PascalCase `{"Error":"..."}`; MVC paths use camelCase `{"error":"..."}`. Off-contract for 500. | Med | Serialize with `JsonNamingPolicy.CamelCase` (or app JSON options). | Pending |
| 5 | `src/SupportTicket.Api/Program.cs` (Swagger setup) | Swagger/OpenAPI enabled; Core AC lists Swagger as Stretch-only / out of Core. | Med | Remove for Core submission, or gate behind Stretch and document intentional deviation. | Pending |
| 6 | `src/SupportTicket.Api/Services/TicketService.cs`, `CommentService.cs` | Services trust validators: ignored `TryParsePriority` return; `createdBy!.Name` can NRE → 500 if filter bypassed. | Low | Defensive guards in service (`ServiceResult` fail on bad parse / missing user). | Pending |
| 7 | `src/SupportTicket.Api/Services/TicketService.cs` (MapToDetail / MapToListItem) | Duplicated field-mapping (~10 fields). | Low | Extract shared private mapper for list fields; detail mapper extends it. | Pending |

**Note:** `.env.example` documenting `Password=password` as a placeholder is fine. The problem is the same string in tracked `appsettings*.json`.

## AI-Assisted Review Summary — Frontend (Prompt 2)

**Prompt used:** `ai-prompts/code-review.md` → Prompt 2

**Date:** 2026-07-23

**Scope reviewed:** Frontend (`src/SupportTicket.Web`) against `ui-flow.md` and `acceptance-criteria.md`

### Review checklist (Prompt 2)

| # | Area | Verdict |
|---|------|---------|
| 1 | Status dropdown — only shows valid next states? | Pass — dropdown populated from `ticket.validNextStatuses` (server-computed); terminal states disable control with helper text |
| 2 | Error handling — invalid transitions shown clearly to user? | Pass — server message surfaced via MUI `Alert` (`statusError`); bypass errors show API message |
| 3 | Search and filter — wired correctly to API query params? | Pass — `getTickets` builds `search`/`status` `URLSearchParams`; search debounced 300ms |
| 4 | Loading and error states on all pages? | Mostly pass — list page shows empty table under error banner (#F2) |
| 5 | No hardcoded API URLs (use env variable)? | Pass — `import.meta.env.VITE_API_URL` with hard fail if unset; documented in `.env.example` |
| 6 | Accessibility basics (labels, form errors)? | Mostly pass — search input missing accessible label (#F1) |

### AI Findings

| # | File | Finding | Severity | Suggested fix | Action |
|---|------|---------|----------|---------------|--------|
| F1 | `src/SupportTicket.Web/src/pages/TicketListPage.tsx` (lines 131–146) | Search `TextField` has only a `placeholder`, no `label` or `aria-label` — screen readers announce no field name. | Med | Add `label="Search"` or `aria-label="Search tickets"`. | Pending |
| F2 | `src/SupportTicket.Web/src/pages/TicketListPage.tsx` (lines 178–202) | On load error, `tickets` is reset to `[]` and render falls through to table branch, showing empty "0 tickets" table under the error banner. | Low | Skip the table when `error` is set (render `null` in that branch). | Pending |
| F3 | `src/SupportTicket.Web/src/pages/CreateTicketPage.tsx` (lines 214–244) | Priority `ToggleButtonGroup` has no group `aria-label` (minor a11y polish; individual buttons carry chip text). | Low | Add `aria-label="Priority"` to the group. | Pending |

**Note:** API validation errors surface as a top-of-form banner rather than inline per-field (ui-flow Flow 2.5 mentions inline errors). Acceptable for Core since the API returns a single `{ error }` message — no change suggested.

## My Review Observations

- _[Your own review notes beyond what AI found]_
- _[Architecture concerns, naming, test gaps]_

## Changes Made After Review

| File | Change | Reason |
|------|--------|--------|
| _[path]_ | _[what changed]_ | _[why]_ |

## Suggestions Rejected (and why)

| AI Suggestion | Why Rejected |
|---------------|--------------|
| _[fill when you decide]_ | _[reason]_ |

## Next step

Use Prompt 3 in `ai-prompts/code-review.md` with only the rows you mark **Accept** in the Action column. Log applied fixes in `review-fixes.md` (or the Changes Made table above).
