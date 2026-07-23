# Support ticket management tests
_Exported on 7/23/2026 at 10:45:47 GMT+5:30 from Cursor (3.12.30)_

---

**User**

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
@requirements-analysis.md @test-strategy.md @api-contract.md 
and update @ai-prompts/design.md  response log

---

**Cursor**

Gathering requirements, API contract, and any existing test strategy to draft the plan.


I have the requirements, API contract, and current skeleton. Creating a plan to expand `test-strategy.md` and update the design prompt response log.


---

**User**

Implement the plan as specified, it is attached for your reference. Do NOT edit the plan file itself.

To-do's from the plan have already been created. Do not create them again. Mark them as in_progress as you work, starting with the first one. Don't stop until you have completed all the to-dos.

---

**Cursor**

Implementing the plan: rewriting `test-strategy.md` and updating the design prompt response log.


Fixing a typo in the transition matrix, then updating the design response log.


Both deliverables are done. Summary:

## What changed

### [test-strategy.md](test-strategy.md)
Replaced the skeleton with a full Core test strategy:

1. **Complete transition matrix** — All 25 from×to pairs (5 valid, 20 invalid) with HTTP expectations, error codes, categories, and mandatory vs recommended priorities
2. **Test infrastructure** — `WebApplicationFactory`, xUnit, PostgreSQL test DB options (dedicated vs Testcontainers), data isolation, helpers, and assertion rules
3. **Status endpoint isolation** — POST/PUT with `status`, invalid enum on PATCH, 404 on missing ticket
4. **Additional integration tests** — Validation (400), not-found (404), and happy-path smoke cases aligned with `acceptance-criteria.md`
5. **Edge-case cross-reference** — Maps to `requirements-analysis.md`; concurrent updates marked as not tested
6. **Out of scope** — Auth, user CRUD, delete, pagination, E2E, performance, etc., with rationale
7. **Clarification** — Same-state transitions return `400` (not idempotent)

### [ai-prompts/design.md](ai-prompts/design.md)
Filled the **Prompt 4 — Test Strategy** Response Log (date 2026-07-23, summary, accepted/changed/why).

## Key decisions captured

| Topic | Decision |
|-------|----------|
| Core minimum | 5 valid + ≥4 invalid integration tests |
| Full coverage | Parameterized tests over all 20 invalid pairs (runtime must reject all per AC-6) |
| Database | Real PostgreSQL — not EF InMemory for state-machine tests |
| Same-state | `400` with `INVALID_TRANSITION` |

No test code was added; that belongs in the implementation phase under `tests/`.

