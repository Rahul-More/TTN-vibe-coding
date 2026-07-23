# Core submission acceptance criteria
_Exported on 7/23/2026 at 10:08:19 GMT+5:30 from Cursor (3.12.30)_

---

**User**

Based on @requirements-analysis.md, help me create a complete acceptance criteria checklist for the Core submission.

Include sections:
- Core features (mapped to the 11 official acceptance criteria from the assessment guide)
- Validation rules
- Error handling expectations
- Testing requirements (state machine integration tests)
- Documentation requirements

Format as markdown checkboxes `- [ ]` that I can paste into acceptance-criteria.md.
Do not implement code.

---

**Cursor**

Gathering requirements and the assessment guide's 11 acceptance criteria to draft the checklist.


Comparing the existing checklist with requirements to identify gaps, then drafting the plan with the full paste-ready checklist.


Drafting the plan with the full paste-ready checklist, aligned to `requirements-analysis.md` and the 11 official criteria in `spec.md`.


---

**User**

Implement the plan as specified, it is attached for your reference. Do NOT edit the plan file itself.

To-do's from the plan have already been created. Do not create them again. Mark them as in_progress as you work, starting with the first one. Don't stop until you have completed all the to-dos.

---

**Cursor**

Implementing the plan: updating `acceptance-criteria.md` first, then filling the Prompt 2 response log.


The plan is implemented. Here is what changed:

### Files modified

1. **[acceptance-criteria.md](acceptance-criteria.md)** — Replaced with the 5-section checklist:
   - **Core Features** — AC-1 through AC-11 plus supporting scope (seeded users, no auth/CRUD, timestamps, etc.)
   - **Validation Rules** — Ticket, comment, query param, and foreign-key rules
   - **Error Handling Expectations** — HTTP codes, error shapes, UI surfacing, edge cases
   - **Testing Requirements** — Integration test infrastructure, 5 valid transitions, invalid transition cases, status endpoint isolation, recommended extras
   - **Documentation Requirements** — README, `.env.example`, setup notes, lifecycle artifacts

2. **[ai-prompts/planning.md](ai-prompts/planning.md)** — Filled the Prompt 2 Response Log with date, summary, accepted/changed items, and rationale.

### Notable improvements over the previous version

- Restructured into the 5 sections requested by Prompt 2 (State Machine and Users folded in)
- Added PUT full-replace semantics, UTC timestamps, and negative scope checkboxes
- Expanded validation and error-handling detail from `requirements-analysis.md` and `api-contract.md`
- Testing section aligned with `test-strategy.md` (representative invalid transitions; AC-6 still requires all invalid transitions to return `400` at runtime)

You can use `acceptance-criteria.md` as your definition of done for the Core submission. Next step in the workflow is **Prompt 3 — Implementation Plan & Milestones** in `ai-prompts/planning.md`.

---

**User**

I need an implementation plan for a Support Ticket Management System:
- Backend: ASP.NET Core 8 Web API
- Frontend: React + TypeScript (Vite)
- Database: PostgreSQL with EF Core

Read @acceptance-criteria.md.

Create a task breakdown with:
1. Ordered tasks with dependencies
2. Milestones for a 7-day self-paced timeline
3. Which tasks should be separate AI chat sessions
4. Risks and mitigations (especially around the status state machine)
5. What is Core vs Stretch (do not include Stretch in the plan)

Output markdown for implementation-plan.md. No code yet.

---

**Cursor**

Reviewing acceptance criteria and existing planning artifacts to draft the implementation plan.


Writing the implementation plan and updating the Prompt 3 response log.


The implementation plan is in [implementation-plan.md](implementation-plan.md). Prompt 3 Response Log in [ai-prompts/planning.md](ai-prompts/planning.md) is updated.

### What's in the plan

**1. Ordered tasks with dependencies** — Five phases, 30+ tasks with IDs and explicit `Depends on` links:
- **Phase 0** — Planning (P0.1–P0.3) — done
- **Phase 1** — Design (D1.1–D1.4) — Day 1
- **Phase 2** — Backend (B2.1–B2.9) — Days 2–3
- **Phase 3** — Frontend (F3.1–F3.8) — Days 4–5
- **Phase 4** — Testing (T4.1–T4.6) — Day 6
- **Phase 5** — Review & submission (R5.1–R5.7) — Day 7

**2. 7-day milestones**

| Day | Milestone | Focus |
|-----|-----------|-------|
| 1 | M1 — Design complete | Architecture, API, UI flows, test strategy |
| 2–3 | M2 — Backend working | DB, state machine, all 7 endpoints |
| 4–5 | M3 — Frontend working | List, create, detail, status, comments |
| 6 | M4 — Tests green | Integration tests + `test-results.md` |
| 7 | M5 — Submission ready | Review, README, reflection |

**3. AI chat sessions** — Mapped to [PROMPT-ORDER.md](PROMPT-ORDER.md): one new chat per prompt (23 sessions total), with notes on what can stay in the same chat (manual checkpoints, running tests).

**4. Risks & mitigations** — Separate tables for:
- **State machine** (highest priority) — scattered logic, extra transitions, PUT vs PATCH confusion, terminal states, same-state no-ops
- Backend/data, frontend, and process/assessment risks

**5. Core vs Stretch** — In-scope table (what this plan covers) and out-of-scope list (auth, pagination, Docker, etc.) with no Stretch tasks in the plan.

Also included: dependency mermaid diagram, AC traceability table, pre-coding checklist, and buffer strategy if you fall behind.

**Next step:** Day 1 — `ai-prompts/design.md` → Prompt 1 (Architecture & Data Model) in a new Cursor chat.

