# Prompt Order — Quick Reference

Use this as your step-by-step guide. **One new Cursor chat per prompt.**

After each prompt, fill in the **Response Log** in that prompt file.

---

| Step | File | Prompt | What you get |
|------|------|--------|--------------|
| 1 | `ai-prompts/planning.md` | Prompt 1 | Refined requirements & edge cases |
| 2 | `ai-prompts/planning.md` | Prompt 2 | Acceptance criteria checklist |
| 3 | `ai-prompts/planning.md` | Prompt 3 | Implementation plan |
| 4 | `ai-prompts/design.md` | Prompt 1 | Architecture & data model |
| 5 | `ai-prompts/design.md` | Prompt 2 | API contract |
| 6 | `ai-prompts/design.md` | Prompt 3 | UI flows |
| 7 | `ai-prompts/design.md` | Prompt 4 | Test strategy |
| 8 | `ai-prompts/implementation.md` | Prompt 1 | DB schema, migrations, seed |
| 9 | `ai-prompts/implementation.md` | Prompt 2 | Status state machine service |
| 10 | `ai-prompts/implementation.md` | Prompt 3 | Backend API |
| 11 | `ai-prompts/implementation.md` | Prompt 4 | Frontend — list & create |
| 12 | `ai-prompts/implementation.md` | Prompt 5 | Frontend — detail & comments |
| 13 | `ai-prompts/testing.md` | Prompt 1 | State machine integration tests |
| 14 | `ai-prompts/testing.md` | Prompt 2 | Validation & error tests |
| 15 | `ai-prompts/testing.md` | Prompt 3 | Coverage vs acceptance criteria |
| 16 | `ai-prompts/debugging.md` | As needed | Fix bugs (use when issues arise) |
| 17 | `ai-prompts/code-review.md` | Prompt 1 | Backend code review |
| 18 | `ai-prompts/code-review.md` | Prompt 2 | Frontend code review |
| 19 | `ai-prompts/code-review.md` | Prompt 3 | Apply accepted fixes |
| 20 | `ai-prompts/documentation.md` | Prompt 1 | README |
| 21 | `ai-prompts/documentation.md` | Prompt 2 | PR description |
| 22 | `ai-prompts/documentation.md` | Prompt 3 | Reflection |
| 23 | `ai-prompts/documentation.md` | Prompt 4 | Pre-submission checklist |

---

## Before Every Chat

1. Open the prompt file for your step
2. Start a **new** Cursor chat (keeps context clean)
3. `@`-reference the files listed in the prompt
4. Copy the entire prompt block into chat
5. After response → log in **Response Log** section

## Persistent Context (reference often)

- `tool-specific/cursor-workflow/project-context.md`
- `tool-specific/cursor-workflow/spec.md`
- `tool-specific/cursor-workflow/cursor-rules-or-instructions.md`

## Track Progress

Update checkboxes in `tool-specific/cursor-workflow/tasks.md` as you complete each step.

## Assessment Artifacts Checklist

All of these are already created — fill them in as you work:

- [x] `candidate-info.md` — fill your name/dates
- [x] `tool-workflow.md` — Part A (review/customize)
- [x] `requirements-analysis.md`
- [x] `acceptance-criteria.md`
- [x] `implementation-plan.md`
- [x] `design-notes.md`
- [x] `api-contract.md`
- [x] `data-model.md`
- [x] `ui-flow.md`
- [x] `test-strategy.md`
- [ ] `test-results.md` — fill after running tests
- [ ] `debugging-notes.md` — fill when bugs occur
- [ ] `code-review-notes.md` — fill after review prompts
- [ ] `review-fixes.md` — fill after applying fixes
- [ ] `pr-description.md` — fill before submission
- [ ] `reflection.md` — fill before submission
- [ ] `final-ai-usage-summary.md` — fill before submission
- [x] `ai-prompts/` — 7 files, 25 prompts total
- [x] `tool-specific/cursor-workflow/` — 4 files
- [ ] `src/` — code from implementation prompts
- [ ] `tests/` — tests from testing prompts
- [ ] `database/` — migrations from implementation prompt 1
