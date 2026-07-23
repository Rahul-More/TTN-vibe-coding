# AI Prompts — Planning Phase

> **How to use:** Start a **new Cursor chat** for each prompt below. Copy the entire prompt block into the chat. After the response, fill in the **Response Log** section.

---

## Prompt 1 — Requirement Analysis & Edge Cases

**When:** Before any coding. First chat session.

**@ references to attach:** `requirements-analysis.md`, `tool-specific/cursor-workflow/project-context.md`

### Copy into chat:

```
I am building a Support Ticket Management System for a .NET AI capability assessment (Option 1 — Backend-Heavy).

Read @requirements-analysis.md and @tool-specific/cursor-workflow/project-context.md.

Help me refine the requirement analysis:

1. List all functional requirements for Core scope only (no auth, no user CRUD UI).
2. Document the status state machine with every valid and invalid transition.
3. Identify at least 10 edge cases with expected behavior.
4. List assumptions I should document for a product owner.
5. Flag anything ambiguous that I should decide before coding.

Output in markdown I can paste into requirements-analysis.md. Do not write code yet.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | |
| **AI response summary** | |
| **Accepted** | |
| **Changed** | |
| **Rejected** | |
| **Why** | |

---

## Prompt 2 — Acceptance Criteria Checklist

**When:** After Prompt 1. New chat session.

**@ references:** `requirements-analysis.md`, `acceptance-criteria.md`

### Copy into chat:

```
Based on @requirements-analysis.md, help me create a complete acceptance criteria checklist for the Core submission.

Include sections:
- Core features (mapped to the 11 official acceptance criteria from the assessment guide)
- Validation rules
- Error handling expectations
- Testing requirements (state machine integration tests)
- Documentation requirements

Format as markdown checkboxes `- [ ]` that I can paste into acceptance-criteria.md.
Do not implement code.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | |
| **AI response summary** | |
| **Accepted** | |
| **Changed** | |
| **Rejected** | |
| **Why** | |

---

## Prompt 3 — Implementation Plan & Milestones

**When:** After acceptance criteria are finalized. New chat session.

**@ references:** `acceptance-criteria.md`, `implementation-plan.md`

### Copy into chat:

```
I need an implementation plan for a Support Ticket Management System:
- Backend: ASP.NET Core 8 Web API
- Frontend: React + TypeScript (Vite)
- Database: PostgreSQL with EF Core

Read @acceptance-criteria.md.

Create a task breakdown with:
1. Ordered tasks with dependencies
2. Milestones for a 7-day self-paced timeline
3. Which tasks should be separate AI chat sessions
4. Risks and mitigations (especially around the status state machine)
5. What is Core vs Stretch (do not include Stretch in the plan)

Output markdown for implementation-plan.md. No code yet.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | |
| **AI response summary** | |
| **Accepted** | |
| **Changed** | |
| **Rejected** | |
| **Why** | |
