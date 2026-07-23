# Status state machine implementation
_Exported on 7/23/2026 at 11:40:29 GMT+5:30 from Cursor (3.12.30)_

---

**User**

@implementation.md (58-76) Implement the status state machine for tickets.

Valid transitions ONLY:
- Open → InProgress
- InProgress → Resolved
- Resolved → Closed
- Open → Cancelled
- InProgress → Cancelled

Read @api-contract.md and @requirements-analysis.md.

Create:
1. StatusTransitionService (or equivalent) — single place for all transition rules
2. Method: bool IsValidTransition(currentStatus, newStatus)
3. Method: IEnumerable<string> GetValidNextStatuses(currentStatus)
4. Throw or return Result for invalid transitions (consistent with project error pattern)
5. Unit-testable design (pure logic, no HTTP)

Do not implement controllers yet. Show me the service and any enums.
@api-contract.md @requirements-analysis.md @design-notes.md 
udpate the response log in @ai-prompts/implementation.md 

---

**Cursor**

I'll read the referenced docs and existing code to align the status state machine with project patterns before drafting a plan.


I have enough context. The `TicketStatus` enum exists; the service layer and error patterns are still greenfield. I'll align the plan with `design-notes.md` and the API contract.


**Invalid transition handling**

> **No Result type exists in the codebase yet. For invalid transitions, which pattern should StatusTransitionService use?**
> - Throw InvalidStatusTransitionException (message + INVALID_TRANSITION code) — TicketService/middleware maps to 400 later
> - Return a lightweight Result/TransitionResult type (no throw) — caller checks success/failure ✔
> - bool IsValidTransition only — TicketService builds error message when false (no throw/Result in service)

You chose the Result pattern — I'll plan a lightweight `TransitionResult` (or shared `ServiceResult`) for invalid transitions.


---

**User**

no minimal api use controllers patterns and use service pattern

---

**Cursor**

Updating the plan to use controllers and the service pattern instead of minimal API. Checking the current plan and project setup.


Updating the plan to require controllers + service-layer architecture (not minimal API) and align DI with that pattern.


Plan updated for **Controllers + Services** (no Minimal API). Changes:

### Architecture
- **Controllers → Services → `StatusTransitionService`** per [`design-notes.md`](design-notes.md)
- **No** `MapGet` / `MapPost` route handlers in `Program.cs`
- Prompt 2 still only implements the state machine service; endpoint controllers stay in Prompt 3

### `Program.cs` (this prompt)
- `builder.Services.AddControllers()`
- `app.MapControllers()` instead of bare `app.Run()`
- `AddScoped<IStatusTransitionService, StatusTransitionService>()` (scoped, not singleton — standard with controllers)

### Unchanged
- `IStatusTransitionService` with `IsValidTransition`, `GetValidNextStatuses`, `ValidateTransition` → `ServiceResult`
- Pure logic, no HTTP/DB in the service
- xUnit matrix tests for all 25 transition pairs

Say **"go ahead"** or **"execute the plan"** when you want this implemented.

---

**User**

Implement the plan as specified, it is attached for your reference. Do NOT edit the plan file itself.

To-do's from the plan have already been created. Do not create them again. Mark them as in_progress as you work, starting with the first one. Don't stop until you have completed all the to-dos.

---

**Cursor**

Implementing the Status State Machine Service per the plan. Starting with `ServiceResult` and the transition service.


Implementation is complete. Summary:

## What changed

### Service layer
- **`ServiceResult`** — lightweight Result type with `Ok()` / `Fail(error, code)`
- **`IStatusTransitionService`** / **`StatusTransitionService`** — single source of truth for transition rules

```1:35:src/SupportTicket.Api/Services/StatusTransitionService.cs
using SupportTicket.Api.Models.Enums;
using SupportTicket.Api.Services.Common;

namespace SupportTicket.Api.Services;

public class StatusTransitionService : IStatusTransitionService
{
    private static readonly IReadOnlyDictionary<TicketStatus, TicketStatus[]> Transitions =
        new Dictionary<TicketStatus, TicketStatus[]>
        {
            [TicketStatus.Open] = [TicketStatus.InProgress, TicketStatus.Cancelled],
            [TicketStatus.InProgress] = [TicketStatus.Resolved, TicketStatus.Cancelled],
            [TicketStatus.Resolved] = [TicketStatus.Closed],
            [TicketStatus.Closed] = [],
            [TicketStatus.Cancelled] = [],
        };

    public bool IsValidTransition(TicketStatus currentStatus, TicketStatus newStatus) =>
        Transitions.TryGetValue(currentStatus, out var targets) && targets.Contains(newStatus);

    public IEnumerable<string> GetValidNextStatuses(TicketStatus currentStatus) =>
        Transitions[currentStatus].Select(s => s.ToString());

    public ServiceResult ValidateTransition(TicketStatus currentStatus, TicketStatus newStatus)
    {
        if (IsValidTransition(currentStatus, newStatus))
        {
            return ServiceResult.Ok();
        }

        return ServiceResult.Fail(
            $"Cannot transition from {currentStatus} to {newStatus}",
            "INVALID_TRANSITION");
    }
}
```

### Enum (unchanged)
`TicketStatus`: `Open`, `InProgress`, `Resolved`, `Closed`, `Cancelled`

### Host setup
`Program.cs` uses **controllers + services** (not minimal API):
- `AddControllers()` / `MapControllers()`
- `AddScoped<IStatusTransitionService, StatusTransitionService>()`

### Tests
- **`tests/SupportTicket.Api.Tests`** — 55 tests passing (25×2 matrix + 5 `GetValidNextStatuses` cases)
- Added to `src/SupportTicket.slnx`

### Docs
- Prompt 2 response log updated in `ai-prompts/implementation.md`

## Files created/modified

| File | Action |
|------|--------|
| `src/SupportTicket.Api/Services/Common/ServiceResult.cs` | Created |
| `src/SupportTicket.Api/Services/IStatusTransitionService.cs` | Created |
| `src/SupportTicket.Api/Services/StatusTransitionService.cs` | Created |
| `src/SupportTicket.Api/Program.cs` | Updated |
| `tests/SupportTicket.Api.Tests/` | Created |
| `src/SupportTicket.slnx` | Updated |
| `ai-prompts/implementation.md` | Updated |

No controllers yet — those come in Prompt 3 when `TicketService` will call `ValidateTransition` and map failures to `400` + `INVALID_TRANSITION`.

