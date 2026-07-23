# Support ticket management requirements
_Exported on 7/23/2026 at 09:15:19 GMT+5:30 from Cursor (3.12.30)_

---

**User**

I am building a Support Ticket Management System for a .NET AI capability assessment (Option 1 — Backend-Heavy).

Read @requirements-analysis.md and @tool-specific/cursor-workflow/project-context.md.

Help me refine the requirement analysis:

1. List all functional requirements for Core scope only (no auth, no user CRUD UI).
2. Document the status state machine with every valid and invalid transition.
3. Identify at least 10 edge cases with expected behavior.
4. List assumptions I should document for a product owner.
5. Flag anything ambiguous that I should decide before coding.

Output in markdown I can paste into requirements-analysis.md. Do not write code yet.

---

**Cursor**

I'll read the requirements analysis and project context files to refine the functional requirements, state machine, and edge cases.


I have enough context from the spec, API contract, and data model. I'll compile the refined requirements analysis as paste-ready markdown.


---

**User**

Implement the plan as specified, it is attached for your reference. Do NOT edit the plan file itself.

To-do's from the plan have already been created. Do not create them again. Mark them as in_progress as you work, starting with the first one. Don't stop until you have completed all the to-dos.

---

**Cursor**

I'll update `requirements-analysis.md` with the refined plan content, then resolve ambiguities and refresh acceptance criteria.


I'll write the full refined `requirements-analysis.md` with a Decisions section, then update `acceptance-criteria.md`.


All three todos are done. Here is what changed:

### [requirements-analysis.md](ai-practical-assessment/requirements-analysis.md)
- Kept the header and **My Understanding** section unchanged
- Replaced everything from **Functional Requirements** onward with the refined plan:
  - Core functional requirements grouped by Tickets, Comments, Users, Validation, Testing
  - Entity table, enums, and API surface
  - Full state machine with valid transitions, invalid transition matrix, and enforcement rules
  - 20 edge cases with resolved expected behaviors (no more "decide" placeholders)
  - 16 product-owner assumptions
  - **Decisions** table locking all 14 ambiguities to recommended defaults

### [acceptance-criteria.md](ai-practical-assessment/acceptance-criteria.md)
- Expanded from the original checklist into a full definition-of-done aligned with finalized requirements:
  - **Core** — 11 official criteria (AC-1 through AC-11), each with specific verifiable detail
  - **Validation** — field rules, enum checks, status-on-PUT rejection, trim behavior
  - **State Machine** — all 5 valid transitions plus invalid transition categories
  - **Error Handling** — HTTP codes and UI behavior
  - **Testing** — integration test coverage for transitions and validation
  - **Documentation** — README, setup notes, lifecycle artifacts
  - **Users** — seeded read-only user requirements

You are ready for **Prompt 3** in `ai-prompts/planning.md` (implementation plan). If you want any decision changed (e.g. block comments on Closed tickets), say which number and I can update both files.

