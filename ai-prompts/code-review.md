# AI Prompts — Code Review Phase

> **How to use:** New chat after implementation is feature-complete. Review before final submission.

---

## Prompt 1 — Full Backend Code Review

**When:** Backend is complete, tests are passing.

**@ references:** `acceptance-criteria.md`, `api-contract.md`, `tool-specific/cursor-workflow/cursor-rules-or-instructions.md`, `src/` backend files

### Copy into chat:

```
Perform a code review of my Support Ticket Management backend.

Read @acceptance-criteria.md and @api-contract.md.

Review for:
1. Status state machine — is logic in ONE place only?
2. Validation — are all required fields checked on backend?
3. Error handling — correct HTTP codes (400, 404, 500)?
4. Security — any hardcoded secrets or connection strings?
5. API contract compliance — do endpoints match api-contract.md?
6. Code quality — naming, separation of concerns, no duplicated logic
7. Over-engineering — flag anything beyond Core scope

Format:
| # | File | Issue | Severity (High/Med/Low) | Suggested fix |

Do not apply fixes yet — I will decide what to accept. I will log results in code-review-notes.md.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | 2026-07-23 |
| **AI response summary** | Backend review against AC + API contract. State machine centralized (`StatusTransitionService`); validation mostly solid; 7 findings logged in `code-review-notes.md`. Highest: committed connection strings in appsettings (AC-10). Also: numeric enum parse gap, 500 PascalCase error body, Swagger beyond Core, minor defensive/mapping notes. No fixes applied. |
| **Accepted fixes** | None yet — all 7 findings left **Pending** in `code-review-notes.md` for decision. |
| **Rejected suggestions** | None yet. |
| **Why rejected** | N/A — awaiting Accept/Reject on each Action row before Prompt 3. |

---

## Prompt 2 — Frontend Code Review

**When:** Frontend is complete.

**@ references:** `ui-flow.md`, `acceptance-criteria.md`, frontend `src/` files

### Copy into chat:

```
Perform a code review of my React frontend for the Support Ticket Management System.

Read @ui-flow.md and @acceptance-criteria.md.

Review for:
1. Status dropdown — only shows valid next states?
2. Error handling — invalid transitions shown clearly to user?
3. Search and filter — wired correctly to API query params?
4. Loading and error states on all pages?
5. No hardcoded API URLs (use env variable)?
6. Accessibility basics (labels, form errors)?

Format findings as a table. Suggest only Core-scope fixes.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | 2026-07-23 |
| **AI response summary** | Frontend review against `ui-flow.md` + `acceptance-criteria.md`. Status dropdown correctly driven by `validNextStatuses`; search/filter and env-based API URL solid; 3 findings logged in `code-review-notes.md` (highest: search input a11y label). No fixes applied. |
| **Accepted fixes** | None yet — all 3 findings left **Pending** in `code-review-notes.md` for decision. |
| **Rejected suggestions** | None yet. |
| **Why rejected** | N/A — awaiting Accept/Reject on each Action row before Prompt 3. |

---

## Prompt 3 — Apply Review Fixes

**When:** After reviewing AI findings. New chat for each batch of fixes.

### Copy into chat:

```
Apply the following code review fixes to my project.

Fixes to apply:
[LIST: copy only the fixes you accepted from Prompt 1 and 2]

Rules:
- Minimal changes only — do not refactor unrelated code
- Do not add Stretch features (auth, pagination, Swagger)
- Run existing tests after changes
- List every file you modify

I will log changes in review-fixes.md.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | |
| **Fixes applied** | |
| **Files changed** | |
| **Tests still pass** | Yes / No |
