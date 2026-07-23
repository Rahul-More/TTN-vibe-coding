# Requirement Analysis

## Selected Project Option

**Option 1 — Backend-Heavy: Support Ticket Management System**

## My Understanding (in your own words)

Internal users need a small app to manage support tickets end-to-end: create tickets, view and update them, add comments, search/filter, and move tickets through a **strict status lifecycle**. Users are seeded in the database (no user-management UI in Core). The hardest requirement is the **status state machine** — invalid transitions must be rejected by the backend and surfaced clearly in the UI.

## Functional Requirements

### Core
- [ ] Create a ticket (title, description, priority, assignee, createdBy)
- [ ] List all tickets
- [ ] View ticket detail with comments
- [ ] Update ticket fields (title, description, priority, assignee)
- [ ] Change status only through valid transitions
- [ ] Add comments to a ticket
- [ ] Keyword search on tickets
- [ ] Filter tickets by status
- [ ] Data persists across application restarts

### Entities

| Entity | Fields |
|--------|--------|
| User (seeded) | id, name, email, role |
| Ticket | id, title, description, priority, status, assignedTo, createdBy, createdAt, updatedAt |
| Comment | id, ticketId, message, createdBy, createdAt |

### Status State Machine
```
Open        → In Progress
In Progress → Resolved
Resolved    → Closed
Open        → Cancelled
In Progress → Cancelled
```
All other transitions are **invalid**.

## Non-Functional Requirements

- Backend validates all required fields; reject invalid input with clear error messages
- UI shows meaningful error states (validation, invalid transition, not found)
- README with local setup instructions
- No secrets committed to the repository
- Integration tests prove state-machine rules

## Assumptions

- Single-tenant internal tool; no multi-organization support in Core
- Users are pre-seeded; `createdBy` and `assignedTo` reference seeded user IDs
- Priority values: Low, Medium, High (or similar enum)
- Authentication is out of scope for Core

## Clarifications (questions for a product owner)

1. Can a ticket be reopened after Resolved/Closed? _(Assumption: No — not in state machine)_
2. Should comments be editable/deletable? _(Assumption: No in Core — append-only)_
3. Is assignee required on create? _(Assumption: Optional)_

## Edge Cases

| Scenario | Expected Behavior |
|----------|-------------------|
| Invalid status transition (e.g. Open → Closed) | Backend returns 400; UI shows error |
| Empty title on create/update | Backend validation error |
| Comment on non-existent ticket | 404 |
| Search with no matches | Empty list, not an error |
| Assign to non-existent user | Backend validation error |
| Concurrent status updates | Last write wins or optimistic concurrency _(document choice)_ |
