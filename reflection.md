# Reflection

## What I Built

A full-stack Support Ticket Management System with .NET 8 Web API, React frontend, and PostgreSQL. Core features include ticket lifecycle management with an enforced status state machine, comments, search/filter, and integration tests.

## How I Used AI (across the lifecycle)

| Phase | AI Tool | Approach |
|-------|---------|----------|
| Requirements | Cursor | Used prompts in `ai-prompts/planning.md` to refine edge cases |
| Design | Cursor | Generated architecture draft; I edited `api-contract.md` manually |
| Implementation | Cursor | Separate chats per layer (DB, API, UI) via `implementation.md` prompts |
| Testing | Cursor | Generated test matrix; I verified against state machine rules |
| Debugging | Cursor | Shared stack traces; validated fixes with `dotnet test` |
| Review | Cursor | AI review prompt; rejected over-scoped suggestions |
| Documentation | Cursor | README and artifact templates |

## What AI Helped With Most

- _[e.g. Scaffolding EF Core entities and migrations quickly]_
- _[e.g. Generating the full transition test matrix]_
- _[e.g. Boilerplate React components and API client]_

## What AI Got Wrong

- _[e.g. Suggested allowing Open → Closed transition]_
- _[e.g. Generated connection string with placeholder password in code instead of env var]_
- _[e.g. Wrong HTTP status code for invalid transition]_

## How I Validated AI Output

- Cross-checked every status transition against `requirements-analysis.md`
- Ran integration tests after every backend change
- Manually tested invalid transitions in UI and via curl/Postman
- Compared API responses to `api-contract.md`
- Reviewed generated code for secrets and over-engineering

## What I Would Improve Next

- _[e.g. Add optimistic concurrency on status updates]_
- _[e.g. Extract reusable Cursor rules for the team]_
- _[e.g. Add E2E tests with Playwright]_

## Reusable Workflow (prompts, rules, specs, templates)

- **Prompt library:** `ai-prompts/` — copy-paste prompts per lifecycle phase
- **Persistent context:** `tool-specific/cursor-workflow/project-context.md`
- **Cursor rules:** `tool-specific/cursor-workflow/cursor-rules-or-instructions.md`
- **Acceptance-driven validation:** Always validate AI output against `acceptance-criteria.md`
