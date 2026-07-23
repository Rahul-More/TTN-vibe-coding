using SupportTicket.Api.Models.Enums;
using SupportTicket.Api.Services.Common;

namespace SupportTicket.Api.Services;

public interface IStatusTransitionService
{
    bool IsValidTransition(TicketStatus currentStatus, TicketStatus newStatus);

    IEnumerable<string> GetValidNextStatuses(TicketStatus currentStatus);

    ServiceResult ValidateTransition(TicketStatus currentStatus, TicketStatus newStatus);
}
