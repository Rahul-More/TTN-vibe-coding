# AI Prompts — Design Phase

> **How to use:** Start a **new Cursor chat** for each prompt. Attach the listed `@` files.

---

## Prompt 1 — Architecture & Data Model

**When:** After planning phase is complete.

**@ references:** `requirements-analysis.md`, `data-model.md`, `design-notes.md`

### Copy into chat:

```
Design the architecture for a Support Ticket Management System (Core scope only).

Stack:
- ASP.NET Core 8 Web API
- React + TypeScript (Vite)
- PostgreSQL + EF Core

Read @requirements-analysis.md.

Provide:
1. High-level architecture diagram (ASCII or mermaid)
2. Backend layer structure (controllers, services, repositories)
3. Complete data model for Users, Tickets, Comments with columns, types, constraints, and indexes
4. Where the status state machine logic should live (single service — explain why)
5. DTO vs entity separation approach

Update content for data-model.md and design-notes.md. No implementation code yet.
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

## Prompt 2 — API Contract

**When:** After data model is agreed. New chat session.

**@ references:** `data-model.md`, `api-contract.md`, `requirements-analysis.md`

### Copy into chat:

```
Design the REST API contract for the Support Ticket Management System.

Entities: Users (read-only list), Tickets (CRUD + status change), Comments (create on ticket).

Requirements:
- GET /tickets with search (keyword) and status filter query params
- PATCH or dedicated endpoint for status changes (state machine enforced)
- Separate update endpoint for non-status fields
- Consistent error response shape
- Document every valid and invalid status transition behavior

Read @data-model.md and @requirements-analysis.md.

Output full api-contract.md content with request/response examples and validation rules.
No implementation code.
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

## Prompt 3 — UI Flows & Frontend Design

**When:** After API contract is done. New chat session.

**@ references:** `api-contract.md`, `ui-flow.md`, `design-notes.md`

### Copy into chat:

```
Design the frontend UI flows for the Support Ticket Management System.

Pages needed (Core):
1. Ticket list — with search and status filter
2. Ticket detail — view/edit fields, change status, view/add comments
3. Create ticket form

Read @api-contract.md.

Provide:
1. Screen map and navigation flow
2. Step-by-step user flows for each feature
3. How the status dropdown should only show valid next states
4. Error states for validation failures and invalid transitions
5. Component breakdown (pages vs reusable components)

Output for ui-flow.md and frontend section of design-notes.md. No code yet.
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

## Prompt 4 — Test Strategy

**When:** After design is complete, before implementation. New chat session.

**@ references:** `requirements-analysis.md`, `test-strategy.md`, `api-contract.md`

### Copy into chat:

```
Create a test strategy for the Support Ticket Management System Core submission.

Mandatory: integration tests proving the status state machine.
- All 5 valid transitions must succeed
- At least 4 invalid transitions must return 400

Read @requirements-analysis.md and @api-contract.md.

Provide:
1. Test matrix (table) for every valid/invalid transition
2. Recommended test setup (WebApplicationFactory, test DB)
3. Additional recommended integration tests (validation, 404)
4. What is NOT in scope for Core and why

Output for test-strategy.md. No test code yet.
```

### Response Log

| Field | Your notes |
|-------|------------|
| **Date** | 2026-07-23 |
| **AI response summary** | Full test-strategy.md: complete 5×5 transition matrix (5 valid, 20 invalid), WebApplicationFactory + PostgreSQL test DB setup, status-endpoint isolation tests, additional validation/404/smoke tests, edge-case cross-ref, Core vs Stretch boundaries; resolved same-state → 400 (not idempotent) |
| **Accepted** | Full transition matrix; mandatory vs recommended test priorities; infrastructure section; alignment with api-contract error shapes |
| **Changed** | Expanded from skeleton; clarified invalid-transition testing uses parameterized approach for full matrix while Core minimum is 5 valid + 4 invalid |
| **Rejected** | — |
| **Why** | Matches locked state machine in requirements-analysis.md and api-contract.md; satisfies AC-11 and Prompt 4 deliverables |
