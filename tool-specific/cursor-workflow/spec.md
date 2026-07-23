# Spec — Support Ticket Management System

## Overview

Build a full-stack support ticket management application for the .NET AI Capability Exercise (Option 1, Core).

## Functional Requirements

### Tickets
- Create ticket: title (required), description, priority, assignee, createdBy
- List tickets with keyword search and status filter
- View ticket detail with comments
- Update ticket fields (not status via this endpoint)
- Change status only through valid transitions
- Default status on create: **Open**

### Comments
- Add comment to ticket (message, createdBy)
- Display comments on ticket detail (newest last or first — document choice)

### Users
- Seeded in database only
- GET /users for dropdown population
- No user management UI

## Status State Machine

```
Open        → InProgress
InProgress  → Resolved
Resolved    → Closed
Open        → Cancelled
InProgress  → Cancelled
```

**All other transitions → 400 Bad Request**

## Non-Functional Requirements

- Data persists across restarts (real database, not in-memory only)
- Backend validates all input
- UI shows API error messages
- Integration tests prove state machine rules
- README with local setup
- No secrets in repository

## API Endpoints (summary)

| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/users | List users |
| GET | /api/tickets | List (+ search, status filter) |
| GET | /api/tickets/{id} | Detail + comments |
| POST | /api/tickets | Create |
| PUT | /api/tickets/{id} | Update fields |
| PATCH | /api/tickets/{id}/status | Change status |
| POST | /api/tickets/{ticketId}/comments | Add comment |

## Acceptance Criteria (11 Core items)

1. Create ticket via UI
2. View all tickets from DB
3. Ticket detail view
4. Update fields and reassign
5. Add comments
6. Valid transitions only
7. Search and status filter
8. Data survives restart
9. Backend validation
10. No secrets in repo
11. State-machine integration tests pass

## Constraints for AI

- Do not add authentication unless I explicitly ask (Stretch)
- Do not over-engineer (no CQRS, MediatR, microservices)
- Keep status logic in one service
- Match `api-contract.md` for endpoint shapes
- Log all AI suggestions; I validate before accepting
