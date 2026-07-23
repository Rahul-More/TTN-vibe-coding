# Support ticket management system
_Exported on 7/23/2026 at 10:31:40 GMT+5:30 from Cursor (3.12.30)_

---

**User**

Design the architecture for a Support Ticket Management System (Core scope only).

Stack:
- ASP.NET Core 8 Web API
- React + TypeScript (Vite)
- PostgreSQL + EF Core

Read @requirements-analysis.md.

Provide:
1. High-level architecture diagram (ASCII or mermaid)
2. Backend layer structure (controllers, services, repositories)
3. Complete data model for Users, Tickets, Comments with columns, types, constraints, and indexes
4. Where the status state machine logic should live (single service — explain why)
5. DTO vs entity separation approach

Update content for data-model.md and design-notes.md. No implementation code yet.
@requirements-analysis.md @data-model.md @design-notes.md 

---

**Cursor**

I'll read the requirements and existing design docs to draft the architecture plan.


Checking the repo structure and related design docs for alignment.


I have the requirements and existing design docs. Creating a plan to update `data-model.md` and `design-notes.md` with the full architecture design.


---

**User**

Implement the plan as specified, it is attached for your reference. Do NOT edit the plan file itself.

To-do's from the plan have already been created. Do not create them again. Mark them as in_progress as you work, starting with the first one. Don't stop until you have completed all the to-dos.

---

**Cursor**

Implementing the architecture design docs per the plan. Starting with `data-model.md`.


Cross-checking both docs against requirements and acceptance criteria.


Cross-check complete — all requirements are covered. Marking remaining todos done.


Architecture design docs are updated per the plan. Summary:

## Files updated

### [data-model.md](data-model.md)
- Mermaid ER diagram plus ASCII relationship summary
- Full **Users**, **Tickets**, and **Comments** specs (PostgreSQL types, constraints, FK ON DELETE rules)
- Index list with rationale per table
- **TicketPriority** / **TicketStatus** enums with C# definitions and EF string conversion
- State machine reference (5 valid transitions, invalid transition rules, `INVALID_TRANSITION`)
- EF entity shape reference and optional dev seed notes

### [design-notes.md](design-notes.md)
- Mermaid system diagram and status-change sequence diagram
- Backend folder layout and layer responsibility table
- All **7 endpoints** mapped controller → service
- **StatusTransitionService** section (API, callers, 5 rationale points, boundaries)
- **DTO vs entity** separation (request/response tables, mapping strategy, validation split)
- Refined frontend, validation, error handling, testing, and Core scope sections

## Design highlights

| Topic | Decision |
|-------|----------|
| State machine | Single `StatusTransitionService` — only place that defines transitions |
| Status writes | PATCH only; POST/PUT DTOs omit `status` |
| Layers | Controllers → Services → Repositories → `AppDbContext` → PostgreSQL |
| DTOs | Manual mapping in services; entities never returned from API |
| UI status dropdown | Driven by `validNextStatuses[]` on ticket detail response |

Cross-checked against `requirements-analysis.md` and `acceptance-criteria.md` — all 7 endpoints, validation rules, field lengths, and state machine rules are covered. No implementation code was added.

