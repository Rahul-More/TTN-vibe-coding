# Support ticket API contract
_Exported on 7/23/2026 at 10:36:08 GMT+5:30 from Cursor (3.12.30)_

---

**User**

Design the REST API contract for the Support Ticket Management System.

Entities: Users (read-only list), Tickets (CRUD + status change), Comments (create on ticket).

Requirements:
- GET /tickets with search (keyword) and status filter query params
- PATCH or dedicated endpoint for status changes (state machine enforced)
- Separate update endpoint for non-status fields
- Consistent error response shape
- Document every valid and invalid status transition behavior

Read @data-model.md and @requirements-analysis.md.

Output full api-contract.md content with request/response examples and validation rules.
No implementation code.

@ai-prompts/design.md @data-model.md @api-contract.md @requirements-analysis.md 

---

**Cursor**

I'll read the referenced design docs and data model to draft a complete REST API contract plan.


I have the domain model, requirements, and the current skeleton contract. Next I'll draft a plan for a full `api-contract.md` that matches the design notes and state machine.


---

**User**

Implement the plan as specified, it is attached for your reference. Do NOT edit the plan file itself.

To-do's from the plan have already been created. Do not create them again. Mark them as in_progress as you work, starting with the first one. Don't stop until you have completed all the to-dos.

---

**Cursor**

I'll implement the full API contract per the plan, starting with conventions and error responses.


Writing the complete `api-contract.md` per the plan.


`api-contract.md` is updated per the plan. Summary:

## What changed

[`api-contract.md`](api-contract.md) is now a full REST contract covering:

1. **Conventions** — Base URL, JSON casing, timestamps, auth model, endpoint summary table
2. **Error standard** — Consistent `{ "error": "..." }` shape with `code` for `INVALID_TRANSITION`; examples for validation, status-on-wrong-endpoint, 404, and 500
3. **Enums & constraints** — `TicketPriority`, `TicketStatus`, and field limits from the data model
4. **Response DTOs** — `UserResponse`, `TicketListItemResponse`, `CommentResponse`, `TicketDetailResponse` (including `validNextStatuses`)
5. **All 7 endpoints** — Full request/response JSON examples, validation rules, and error cases:
   - `GET /users`
   - `GET /tickets` (search, status filter, empty results, invalid status)
   - `GET /tickets/{id}` (comments oldest-first, terminal state)
   - `POST /tickets` (server defaults status to `Open`)
   - `PUT /tickets/{id}` (full replace, status rejected, allowed on Closed/Cancelled)
   - `PATCH /tickets/{id}/status` (sole status mutation path)
   - `POST /tickets/{ticketId}/comments` (allowed on any status)
6. **Status state machine** — Mermaid diagram, all 5 valid transitions with examples, all 20 invalid transitions grouped by source state, `validNextStatuses` lookup table
7. **Reference tables** — Validation rules summary and edge-case cross-reference (#1–20 from requirements)

No implementation code was added. The contract aligns with `data-model.md`, `requirements-analysis.md`, and `design-notes.md`.

