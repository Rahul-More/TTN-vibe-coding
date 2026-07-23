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
