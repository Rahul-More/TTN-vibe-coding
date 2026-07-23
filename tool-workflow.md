# Tool Workflow

## 1. Primary AI Tool Used

**Cursor** — used as the primary AI pair-programming assistant throughout the assessment.

## 2. How I Provide Project Context to the Tool

- **Persistent context files** in `tool-specific/cursor-workflow/` (`project-context.md`, `spec.md`, `tasks.md`).
- **@ references** to key files when starting a new chat: `requirements-analysis.md`, `api-contract.md`, `data-model.md`.
- **Cursor rules** in `tool-specific/cursor-workflow/cursor-rules-or-instructions.md` for stack conventions and constraints.
- **Structured prompts** from `ai-prompts/` — one lifecycle phase per chat session to keep context focused.

## 3. How I Use AI Across the Lifecycle

| Phase | How AI Is Used | Artifact |
|-------|----------------|----------|
| Requirement analysis | Clarify entities, state machine rules, edge cases | `requirements-analysis.md`, `ai-prompts/planning.md` |
| Planning / design | Architecture, API contract, data model, UI flows | `implementation-plan.md`, `design-notes.md`, `ai-prompts/design.md` |
| Code generation | Scaffold API, services, React pages, EF migrations | `src/`, `ai-prompts/implementation.md` |
| Validation | Review against acceptance criteria; reject invalid AI suggestions | `acceptance-criteria.md`, `code-review-notes.md` |
| Testing | Integration tests for state machine; test strategy | `tests/`, `ai-prompts/testing.md` |
| Debugging | Investigate failures with AI; validate fixes manually | `debugging-notes.md`, `ai-prompts/debugging.md` |
| Code review | AI-assisted review; I accept/reject with reasoning | `code-review-notes.md`, `review-fixes.md` |
| Documentation | README, setup notes, PR description | `ai-prompts/documentation.md` |

## 4. What I Avoid Sharing Unnecessarily

- Real credentials, API keys, connection strings with passwords
- Client or company proprietary data unrelated to this exercise
- Personal identifiable information (use seeded fake users only)
- Internal infrastructure hostnames or VPN details

## 5. How I Would Reuse This Workflow in a Real Project

1. Create `project-context.md` and `spec.md` before the first line of code.
2. Break work into **separate chat sessions** per lifecycle phase (see `ai-prompts/`).
3. Log every meaningful prompt and what was accepted/rejected in `ai-prompts/`.
4. Run AI-assisted code review before every PR.
5. Keep `acceptance-criteria.md` as the source of truth for validation prompts.
6. Use Cursor rules to enforce team conventions (naming, error handling, no secrets in repo).
