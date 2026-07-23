# Design Notes

## Architecture Overview

```
┌─────────────┐     HTTP/JSON      ┌──────────────────┐     EF Core     ┌────────────┐
│  React UI   │ ◄──────────────► │  ASP.NET Core    │ ◄─────────────► │ PostgreSQL │
│  (Vite+TS)  │                   │  Web API         │                 │            │
└─────────────┘                   └──────────────────┘                 └────────────┘
                                         │
                                  TicketService
                                  StatusTransitionService
                                  CommentService
```

## Frontend Design

- **Pages:** Ticket list, Ticket detail, Create ticket
- **List:** Table/cards with status filter dropdown + keyword search input
- **Detail:** Ticket fields (editable), status dropdown (only valid next states), comment thread + add comment form
- **Error handling:** Toast or inline alert for API errors (especially invalid transitions)
- **State:** React Query or fetch + useState for simplicity

## Backend Design

- **Layers:** Controllers → Services → Repositories/DbContext
- **StatusTransitionService:** Single place enforcing state machine rules
- **Validation:** FluentValidation or DataAnnotations on DTOs + service-level checks
- **DTOs:** Separate request/response models from entities
- **Error responses:** Consistent shape `{ "error": "...", "code": "INVALID_TRANSITION" }`

## Database Design

See `data-model.md` for full schema.

- `Users` — seeded, referenced by tickets and comments
- `Tickets` — FK to Users (assignedTo, createdBy)
- `Comments` — FK to Tickets and Users (createdBy)
- Indexes on `Tickets.Status`, `Tickets.Title` (for search/filter)

## Validation Strategy

| Layer | What Is Validated |
|-------|-------------------|
| API DTO | Required fields, string lengths, enum values |
| Service | Status transitions, user FK existence |
| Database | FK constraints, NOT NULL on required columns |

## Error Handling Strategy

| HTTP Code | When |
|-----------|------|
| 400 | Validation failure, invalid status transition |
| 404 | Ticket/comment not found |
| 500 | Unhandled exception (logged server-side) |

## Testing Strategy Link

See `test-strategy.md` — focus on integration tests for the state machine in Core.
