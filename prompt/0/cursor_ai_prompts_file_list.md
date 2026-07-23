# AI prompts file list
_Exported on 7/23/2026 at 10:21:15 GMT+5:30 from Cursor (3.12.30)_

---

**User**

 I have covered only @ai-prompts/planning.md prompts from @ai-prompts, create a list of files should I follow 

---

**Cursor**

Exploring the `ai-prompts` folder and related docs to recommend what to follow after `planning.md`.


Quickly scanning the remaining prompt files for their structure and outputs.


You’ve finished the **planning** phase (`ai-prompts/planning.md`, steps 1–3). The repo’s master sequence is in [`PROMPT-ORDER.md`](PROMPT-ORDER.md). Here’s what to follow next.

## Recommended order (after planning)

| Step | Prompt file | Prompts | Main outputs |
|------|-------------|---------|--------------|
| **4–7** | [`ai-prompts/design.md`](ai-prompts/design.md) | 4 | `design-notes.md`, `data-model.md`, `api-contract.md`, `ui-flow.md`, `test-strategy.md` |
| **8–12** | [`ai-prompts/implementation.md`](ai-prompts/implementation.md) | 5 | `src/`, `database/`, working API + React UI |
| **13–15** | [`ai-prompts/testing.md`](ai-prompts/testing.md) | 3 | `tests/`, `test-results.md` |
| **16** | [`ai-prompts/debugging.md`](ai-prompts/debugging.md) | As needed | `debugging-notes.md` (only when bugs appear) |
| **17–19** | [`ai-prompts/code-review.md`](ai-prompts/code-review.md) | 3 | `code-review-notes.md`, `review-fixes.md` |
| **20–23** | [`ai-prompts/documentation.md`](ai-prompts/documentation.md) | 4 | `README.md`, `pr-description.md`, `reflection.md`, `final-ai-usage-summary.md` |

**Next up:** [`ai-prompts/design.md`](ai-prompts/design.md) — Prompt 1 (Architecture & Data Model).

---

## Design phase detail (steps 4–7)

| Prompt | `@` files to attach | Fills in |
|--------|---------------------|----------|
| 1 — Architecture & Data Model | `requirements-analysis.md`, `data-model.md`, `design-notes.md` | `design-notes.md`, `data-model.md` |
| 2 — API Contract | `requirements-analysis.md`, `api-contract.md` | `api-contract.md` |
| 3 — UI Flows | `requirements-analysis.md`, `ui-flow.md` | `ui-flow.md` |
| 4 — Test Strategy | `acceptance-criteria.md`, `test-strategy.md` | `test-strategy.md` |

---

## Files to keep handy (every phase)

**Master guide**
- [`PROMPT-ORDER.md`](PROMPT-ORDER.md) — step-by-step checklist (steps 1–23)
- [`tool-specific/cursor-workflow/tasks.md`](tool-specific/cursor-workflow/tasks.md) — tick off progress

**Persistent context** (reference often)
- [`tool-specific/cursor-workflow/project-context.md`](tool-specific/cursor-workflow/project-context.md)
- [`tool-specific/cursor-workflow/spec.md`](tool-specific/cursor-workflow/spec.md)
- [`tool-specific/cursor-workflow/cursor-rules-or-instructions.md`](tool-specific/cursor-workflow/cursor-rules-or-instructions.md)

**Source of truth from planning**
- [`requirements-analysis.md`](requirements-analysis.md)
- [`acceptance-criteria.md`](acceptance-criteria.md)
- [`implementation-plan.md`](implementation-plan.md)

---

## Workflow reminder

For each prompt:

1. Open the prompt file for that step  
2. Start a **new** Cursor chat  
3. `@`-reference the files listed in that prompt  
4. Copy the full prompt block into chat  
5. Fill in the **Response Log** in that prompt file  

---

## Summary

| Already done | Next | Then |
|--------------|------|------|
| `ai-prompts/planning.md` (3 prompts) | `ai-prompts/design.md` (4 prompts) | `implementation.md` → `testing.md` → (`debugging.md` as needed) → `code-review.md` → `documentation.md` |

That’s **22 more prompts** across 6 files (25 total in `ai-prompts/`). Use [`PROMPT-ORDER.md`](PROMPT-ORDER.md) as your single checklist and update [`tool-specific/cursor-workflow/tasks.md`](tool-specific/cursor-workflow/tasks.md) as you go.

