namespace SupportTicket.Api.DTOs.Responses;

public class TicketDetailResponse : TicketListItemResponse
{
    public IReadOnlyList<string> ValidNextStatuses { get; set; } = [];
    public IReadOnlyList<CommentResponse> Comments { get; set; } = [];
}
