# Code review fixes application
_Exported on 7/23/2026 at 23:22:18 GMT+5:30 from Cursor (3.12.30)_

---

**User**

@code-review.md (90-101) Apply the following code review fixes to my project.

Fixes to apply:
[LIST: copy only the fixes you accepted from Prompt 1 and 2]


@code-review-notes.md
Rules:
- Minimal changes only — do not refactor unrelated code
- Do not add Stretch features (auth, pagination, Swagger)
- Run existing tests after changes
- List every file you modify

I will log changes in @review-fixes.md 

update response log in @ai-prompts/code-review.md 

---

**Cursor**

> Auto routed to grok-4.5



I'll read the review notes and related docs to identify which accepted fixes to apply, then draft a concrete plan.

All review findings are still **Pending** — Prompt 3 needs an explicit Accept list. Checking `review-fixes.md` and the affected files so I can ask a focused accept/reject question.

All findings in [`code-review-notes.md`](code-review-notes.md) are still **Pending**, and Prompt 3’s placeholder (`[LIST: copy only the fixes you accepted…]`) was never filled in. I can’t plan Prompt 3 without knowing which rows you Accept.

Reply with the IDs to apply (or “all Core-aligned”), for example:

**Backend**
- **#1** High — Remove `ConnectionStrings` from committed appsettings (AC-10)
- **#2** Med — Drop hardcoded fallback in `AppDbContextFactory`
- **#3** Med — Reject numeric / undefined enum parse values
- **#4** Med — CamelCase JSON for 500 error body
- **#5** Med — Remove/gate Swagger (Stretch)
- **#6** Low — Defensive guards in services
- **#7** Low — Deduplicate ticket mappers

**Frontend**
- **F1** Med — Search field accessible label
- **F2** Low — Don’t show empty table under load error
- **F3** Low — `aria-label` on Priority toggle group

**Suggested default** if you want a Core-focused batch: accept **#1–#4 + F1–F3**, reject **#5** (Swagger = Stretch), and leave **#6/#7** for later (low / optional cleanup).

Which list should Prompt 3 apply?

---

**User**

Backend

#1 High — Remove ConnectionStrings from committed appsettings (AC-10)
#2 Med — Drop hardcoded fallback in AppDbContextFactory
#3 Med — Reject numeric / undefined enum parse values
#4 Med — CamelCase JSON for 500 error body
#5 Med — Remove/gate Swagger (Stretch)
#6 Low — Defensive guards in services
#7 Low — Deduplicate ticket mappers
Frontend

F1 Med — Search field accessible label
F2 Low — Don’t show empty table under load error
F3 Low — aria-label on Priority toggle group

work on these issues

---

**Cursor**

> Auto routed to grok-4.5



You've accepted all ten findings. I'll inspect the affected backend and frontend files so the plan can specify exact, minimal edits.

I have enough detail. Creating a concrete plan for all ten accepted fixes, plus the Prompt 3 response log update.

---

**User**

Implement the plan as specified, it is attached for your reference. Do NOT edit the plan file itself.

To-do's from the plan have already been created. Do not create them again. Mark them as in_progress as you work, starting with the first one. Don't stop until you have completed all the to-dos.

---

**Cursor**

> Auto routed to composer-2.5



Implementing the plan, starting with backend security/config fixes.


Applying backend and frontend changes.


All 78 tests passed. Updating the tracking docs.


All 10 accepted review fixes are applied. **78/78** API tests pass.

### Backend (#1–#7)
- **#1** — Removed `ConnectionStrings` from `appsettings.json` and `appsettings.Development.json`
- **#2** — `AppDbContextFactory` throws if `ConnectionStrings__DefaultConnection` is unset
- **#3** — `EnumParsing` now requires `Enum.IsDefined` after `TryParse`
- **#4** — 500 responses use camelCase JSON (`{"error":"..."}`)
- **#5** — Swagger removed from `Program.cs`; Swashbuckle removed from `.csproj`
- **#6** — `TicketService` fails on bad priority parse; `CommentService` guards null `createdBy`
- **#7** — Shared `MapListFields` helper for list/detail mapping

### Frontend (F1–F3)
- **F1** — Search field has `label="Search"`
- **F2** — Table/empty state skipped when `error` is set
- **F3** — Priority toggle group and buttons have `aria-label`s

### Docs updated
- [`ai-prompts/code-review.md`](ai-prompts/code-review.md) — Prompt 3 response log filled
- [`code-review-notes.md`](code-review-notes.md) — All actions marked **Accept**; changes table filled
- [`review-fixes.md`](review-fixes.md) — 10 fix rows logged

**Note:** Local API runs need `ConnectionStrings__DefaultConnection` set (see `.env.example`). Integration tests inject it via `CustomWebApplicationFactory`.

