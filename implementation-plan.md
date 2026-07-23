# Implementation Plan

## Overview

Build Option 1 (Support Ticket Management) as a .NET 8 Web API + React frontend + PostgreSQL, delivered in phases aligned with separate AI chat sessions.

## Task Breakdown

| # | Task | Depends On | Chat Prompt File |
|---|------|------------|------------------|
| 1 | Requirement analysis & acceptance criteria | — | `ai-prompts/planning.md` → Prompt 1 |
| 2 | Architecture & data model design | 1 | `ai-prompts/design.md` → Prompt 1–2 |
| 3 | API contract & UI flows | 2 | `ai-prompts/design.md` → Prompt 3 |
| 4 | Database schema, migrations, seed data | 2 | `ai-prompts/implementation.md` → Prompt 1 |
| 5 | Backend API (CRUD, state machine, validation) | 4 | `ai-prompts/implementation.md` → Prompt 2–3 |
| 6 | Frontend (list, detail, create, comments, search/filter) | 5 | `ai-prompts/implementation.md` → Prompt 4–5 |
| 7 | Integration tests (state machine) | 5 | `ai-prompts/testing.md` → Prompt 1–2 |
| 8 | Debugging & fixes | 6–7 | `ai-prompts/debugging.md` |
| 9 | Code review & polish | 8 | `ai-prompts/code-review.md` |
| 10 | Documentation & reflection | 9 | `ai-prompts/documentation.md` |

## Milestones

| Milestone | Deliverable | Target |
|-----------|-------------|--------|
| M1 — Design complete | `design-notes.md`, `api-contract.md`, `data-model.md` filled | Day 1–2 |
| M2 — Backend working | API + DB + state machine | Day 3–4 |
| M3 — Frontend working | Full UI wired to API | Day 4–5 |
| M4 — Tests green | State-machine integration tests pass | Day 5–6 |
| M5 — Submission ready | All artifacts + reflection + PR description | Day 7 |

## AI Usage Plan

- **One new Cursor chat per prompt** in `ai-prompts/` (keeps context clean).
- Always `@`-reference `project-context.md` and relevant design docs.
- After each AI response: log accepted/changed/rejected in the prompt log section of that file.
- Validate AI output against `acceptance-criteria.md` before moving to next prompt.

## Risks

| Risk | Mitigation |
|------|------------|
| AI suggests invalid state transitions | Enforce rules in a dedicated service; test every transition |
| Over-scoping into Stretch features | Stick to Core checklist; mark Stretch as optional |
| Shallow prompt history | Copy prompts from `ai-prompts/` and log responses immediately |
| Secrets in repo | Use `.env.example` only; add `.gitignore` for real env files |

## Mitigation

- Review `acceptance-criteria.md` after each milestone.
- Run integration tests before frontend polish.
- Keep commits small and descriptive for submission form questions.
