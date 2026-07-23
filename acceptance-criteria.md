# Acceptance Criteria

## Core

- [ ] A user can create a ticket via the UI
- [ ] A user can view all tickets from the database
- [ ] A user can open a ticket detail view
- [ ] A user can update ticket fields and reassign
- [ ] A user can add comments
- [ ] Status changes only through valid transitions; invalid ones are rejected
- [ ] Keyword search and status filter work
- [ ] Data remains available after restart
- [ ] Backend validation prevents invalid records
- [ ] No secrets committed to the repo
- [ ] State-machine integration tests pass

## Validation

- [ ] Required fields (title, etc.) enforced on backend
- [ ] Invalid enum values (priority, status) rejected
- [ ] Foreign keys validated (assignedTo, createdBy, ticketId)
- [ ] API returns consistent error response shape

## Error Handling

- [ ] 400 for validation errors and invalid transitions
- [ ] 404 for missing ticket/comment resources
- [ ] 500 handled gracefully without leaking stack traces to client
- [ ] UI displays API error messages to the user

## Testing

- [ ] Integration tests: valid transitions succeed
- [ ] Integration tests: invalid transitions return 400
- [ ] Tests run via documented command in README

## Documentation

- [ ] README with setup and run instructions
- [ ] Database setup documented in `database/setup-notes.md`
- [ ] All lifecycle artifacts present in repository
- [ ] Full prompt history in `ai-prompts/`

## Users (seeded, read-only)

- [ ] 3–5 users seeded in database with id, name, email, role
- [ ] `GET /api/users` returns seeded users for UI dropdowns
- [ ] No user create/update/delete endpoints or UI in Core
