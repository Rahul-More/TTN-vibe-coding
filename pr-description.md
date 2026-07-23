# PR Description

## Summary

Implements Core features for the Support Ticket Management System: ticket CRUD, status state machine, comments, search/filter, backend validation, and state-machine integration tests.

## Features Implemented

- Create, list, view, and update tickets
- Enforced status state machine (invalid transitions rejected)
- Add comments to tickets
- Keyword search and status filter
- Seeded users for assignee/createdBy
- PostgreSQL persistence with migrations and seed data
- Integration tests for all valid/invalid status transitions

## Technical Changes

### Backend
- ASP.NET Core Web API with layered architecture
- `StatusTransitionService` for state machine enforcement
- EF Core entities and migrations
- FluentValidation / DataAnnotations on DTOs

### Frontend
- React + TypeScript ticket list, detail, and create pages
- Status dropdown limited to valid next states
- Error handling for API validation and transition failures

### Database
- Users, Tickets, Comments tables
- Seed data for users and sample tickets

## Database Changes

- Initial migration: `database/schema-or-migrations/`
- Seed script: `database/seed-data/`

## Testing Done

- [ ] State machine integration tests (valid transitions)
- [ ] State machine integration tests (invalid transitions)
- [ ] Create ticket validation test
- [ ] Manual UI walkthrough

## AI Usage Summary

- Planning and design: `ai-prompts/planning.md`, `ai-prompts/design.md`
- Implementation: `ai-prompts/implementation.md`
- Testing: `ai-prompts/testing.md`
- Review: `ai-prompts/code-review.md`
- Full log: `final-ai-usage-summary.md`

## Screenshots / Demo Notes

_[Add screenshots or describe demo steps]_

1. Open ticket list — see seeded tickets
2. Create a new ticket
3. Change status Open → In Progress → Resolved → Closed
4. Try invalid transition Open → Closed — see error
5. Search and filter by status

## Known Limitations

- No authentication (Core scope)
- No user management UI (users are seeded)
- No pagination (Stretch)

## Future Improvements (Stretch)

- JWT authentication and role-based access
- Pagination, sorting, filter by priority/assignee
- Swagger/OpenAPI documentation
- Docker + CI pipeline
- Unit tests for all services
