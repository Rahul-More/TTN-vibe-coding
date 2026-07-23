# Cursor Rules & Instructions

Use these as Cursor rules (`.cursor/rules/`) or paste into Cursor **Rules for AI** settings.

---

## Project Rules

```
You are helping build a Support Ticket Management System for a .NET AI capability assessment.

Stack: ASP.NET Core 8 Web API, React + TypeScript (Vite), PostgreSQL, EF Core, xUnit.

ALWAYS:
- Read project-context.md and spec.md before implementing
- Enforce the ticket status state machine in a single StatusTransitionService
- Validate all input on the backend
- Return 400 for invalid transitions with { "error": "...", "code": "INVALID_TRANSITION" }
- Use environment variables for connection strings — never hardcode secrets
- Match api-contract.md for endpoint shapes
- Keep changes minimal and focused — no unrelated refactors

NEVER (unless I explicitly ask for Stretch):
- Add authentication or JWT
- Add user management UI
- Add pagination, Swagger, Docker, CI
- Use MediatR, CQRS, or microservice patterns
- Commit secrets or real connection strings

CODE STYLE:
- C#: PascalCase public members, async/await for I/O, thin controllers
- TypeScript: functional components, explicit types, env var for API URL
- Errors: consistent JSON error shape across all endpoints

TESTING:
- Integration tests required for all valid and invalid status transitions
- Use WebApplicationFactory with test database

When suggesting changes, explain trade-offs. When unsure, ask before adding scope.
```

---

## How to Use in Cursor

### Option A — Project Rules file

Create `.cursor/rules/assessment.mdc` in the repo root with the rules above.

### Option B — Chat context

At the start of each implementation chat, `@`-reference:
- `tool-specific/cursor-workflow/project-context.md`
- `tool-specific/cursor-workflow/spec.md`
- `tool-specific/cursor-workflow/cursor-rules-or-instructions.md`

### Option C — Copy prompts from ai-prompts/

Each file in `ai-prompts/` has ready-to-paste prompts designed for separate chat sessions.

---

## Persistent Context Files

| File | When to reference |
|------|-------------------|
| `project-context.md` | Every chat |
| `spec.md` | Implementation and review chats |
| `tasks.md` | Track progress; update checkboxes |
| `api-contract.md` | Backend and frontend implementation |
| `acceptance-criteria.md` | Validation and pre-submission |
