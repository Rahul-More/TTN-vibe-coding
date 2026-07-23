namespace SupportTicket.Api.DTOs.Requests;

public class CreateCommentRequest
{
    public string Message { get; set; } = string.Empty;
    public int CreatedBy { get; set; }
}
