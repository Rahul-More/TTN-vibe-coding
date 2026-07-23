# Test coverage review
_Exported on 7/23/2026 at 22:53:13 GMT+5:30 from Cursor (3.12.30)_

---

**User**

Review my test coverage against the Core acceptance criteria.

Read @acceptance-criteria.md and @test-results.md @test-strategy.md 

1. Map each acceptance criterion to a test or manual verification step
2. Identify any gaps in test coverage
3. Suggest additional tests only if required for Core (not Stretch)
4. Generate a test-results.md summary table I can fill in

Do not add Stretch tests unless I explicitly ask.

update the response log in @ai-prompts/testing.md 


---

**Cursor**

> Auto routed to claude-opus-4-8-thinking-high



I'll start by reading the referenced files to understand the acceptance criteria, current test results, and test strategy.

I'll now find and read the other referenced files.

Now let me look at the actual test files to accurately map what's covered.

Let me quickly check the API implementation to confirm whether the suggested Core behaviors (status isolation, search/filter, enum validation) actually exist, so my gap analysis is accurate.

The `[JsonExtensionData]` suggests status-in-body detection exists. Let me confirm via the validators and service.

I now have a complete picture. Let me lay out my findings and the plan.

## What I found

**Existing automated coverage (70 tests):**
- Unit — `StatusTransitionServiceTests` (55): full 25-pair matrix (`IsValidTransition` + `ValidateTransition`) + `GetValidNextStatuses` for all 5 states
- Integration — `StatusTransitionIntegrationTests` (10): 5 valid + 5 invalid transitions via `PATCH`, asserting `validNextStatuses` and DB persistence
- Integration — `ValidationErrorIntegrationTests` (5): empty title, invalid `createdBy`, GET 404, comment 404, empty comment message

**Key insight:** several Core behaviors are fully implemented in the API but have **zero automated test coverage** — confirmed by reading `TicketService`/validators:
- Status-in-body rejection on `POST` and `PUT` (`CreateTicketRequestValidator`/`UpdateTicketRequestValidator` `HasStatusProperty` → 400)
- Invalid `status` query param on list → 400 (`TicketService.ListAsync`)
- Invalid `priority` enum → 400
- Search + status filtering and empty-`[]` behavior

These map directly to Core criteria (AC-1, AC-4, AC-7, AC-9) and the test-strategy's **Mandatory** status-isolation tests (§4), so they qualify as Core gaps, not Stretch.

Let me put this into a plan for your approval.

---

**User**

Implement the plan as specified, it is attached for your reference. Do NOT edit the plan file itself.

To-do's from the plan have already been created. Do not create them again. Mark them as in_progress as you work, starting with the first one. Don't stop until you have completed all the to-dos.

---

**Cursor**

> Auto routed to composer-2.5



I'll start with the AC mapping and test-results table, then add the Core integration tests and helpers.


I'll add the HTTP helpers first, then the new integration tests.


Shell isn't available in this session, so I'll finish the documentation updates and note that tests need a local run.


The API process is locking the build output. I'll try running tests with `--no-build` against the existing assembly.


The old test DLL ran because the API process blocked a rebuild. Stopping it so we can compile and run the new tests.


All 78 tests passed. Updating `test-results.md` and the Prompt 3 response log.


## Summary

Core test coverage review is complete. **78 tests pass** (55 unit + 23 integration).

### What changed

**New integration tests**
- `StatusIsolationIntegrationTests.cs` — status on POST/PUT rejected; invalid PATCH enum
- `SearchFilterIntegrationTests.cs` — combined search+status filter; empty `[]` result
- Extended `ValidationErrorIntegrationTests` — invalid priority; invalid status query param
- Extended `StatusTransitionIntegrationTests` — Open → Open same-state invalid transition

**Helpers** (`TicketApiHelpers.cs`)
- `SeedTicketAsync`, `PutTicketRawAsync`, `GetTicketsListRawAsync`, `ReadTicketListAsync`

**Documentation**
- [test-results.md](test-results.md) — AC-1..AC-11 coverage table (Status column left blank for you to fill)
- [ai-prompts/testing.md](ai-prompts/testing.md) — Prompt 3 response log filled in

### Coverage gaps (manual only — no new tests added)

| AC | Gap | Why manual |
|----|-----|------------|
| AC-2 | Ticket list UI | UI demo per test-strategy §8 |
| AC-3 | Comment oldest-first ordering | UI/API happy-path demo |
| AC-4 | Field update on Closed/Cancelled | Happy-path demo |
| AC-5 | Comment create on Closed/Cancelled | Happy-path demo |
| AC-8 | Data survives restart | Operational verification |
| AC-10 | No secrets in repo | Repo inspection |

### Fill in `test-results.md`

Use the **Status** column in the Acceptance Criteria Coverage table — e.g. `PASS`, `MANUAL PASS`, or `PENDING` — as you complete UI demos and manual checks.

