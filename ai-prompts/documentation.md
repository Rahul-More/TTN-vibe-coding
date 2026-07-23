# AI Prompts — Documentation Phase

> **How to use:** Final chats before submission. Fill in remaining artifact files.

---

## Prompt 1 — README & Setup Instructions

**When:** App runs locally end-to-end.

**@ references:** `database/setup-notes.md`, `candidate-info.md`, `README.md`

### Copy into chat:

```
Write a complete README.md for my Support Ticket Management System submission.

Read @database/setup-notes.md and @candidate-info.md.

Include:
1. Project overview (1 paragraph)
2. Tech stack
3. Prerequisites (.NET 8 SDK, Node.js, PostgreSQL)
4. Step-by-step setup (clone, env vars, DB migrate, seed, run API, run frontend)
5. How to run tests (dotnet test)
6. Environment variables table (.env.example values, no real secrets)
7. API base URL and default ports
8. Link to lifecycle artifacts (requirements-analysis.md, etc.)

Write for someone who has never seen the project before.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | |
| **AI response summary** | |
| **Accepted** | |
| **Changed** | |
| **Rejected** | |

---

## Prompt 2 — PR Description

**When:** Ready to submit.

**@ references:** `pr-description.md`, `acceptance-criteria.md`, `test-results.md`

### Copy into chat:

```
Write a PR description for my Support Ticket Management assessment submission.

Read @acceptance-criteria.md and @test-results.md.

Include:
- Summary
- Features implemented (Core only)
- Technical changes (backend, frontend, database)
- Testing done (with test counts)
- AI usage summary (reference ai-prompts/ folder)
- Known limitations
- Future improvements (Stretch only — clearly labeled)

Output markdown for pr-description.md.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | |
| **AI response summary** | |
| **Accepted** | |
| **Changed** | |

---

## Prompt 3 — Reflection & Final AI Usage Summary

**When:** Last step before submission form.

**@ references:** `reflection.md`, `final-ai-usage-summary.md`, all `ai-prompts/` files

### Copy into chat:

```
Help me write an honest reflection for my .NET AI Capability assessment.

I will provide notes — turn them into reflection.md and final-ai-usage-summary.md content.

My notes:
[PASTE YOUR NOTES HERE, for example:]
- What I built
- How I used AI in each phase
- What AI helped with most
- 2-3 things AI got wrong and how I caught them
- How I validated AI output
- What I would improve next
- Which prompts from ai-prompts/ I used and what I accepted/rejected

Be honest and specific — the assessment values reflection over perfect code.
Do not exaggerate AI contributions.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | |
| **AI response summary** | |
| **Accepted** | |
| **Changed (added my own voice)** | |

---

## Prompt 4 — Pre-Submission Checklist

**When:** Final validation before submitting the form.

**@ references:** `acceptance-criteria.md`, repository root

### Copy into chat:

```
Run a pre-submission checklist against the .NET AI Capability Exercise requirements.

Read @acceptance-criteria.md.

Verify my repository has:
1. All required markdown artifacts at root
2. ai-prompts/ with prompt history logged
3. tool-specific/cursor-workflow/ for Cursor
4. database/ with migrations and seed data
5. src/ with working frontend and backend
6. tests/ with state machine integration tests
7. README with setup instructions
8. No secrets in the repo

List anything missing or incomplete as a checklist I can fix before submitting.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | |
| **Missing items** | |
| **Fixed before submit** | |
