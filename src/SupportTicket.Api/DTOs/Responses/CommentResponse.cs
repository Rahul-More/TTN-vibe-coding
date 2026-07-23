namespace SupportTicket.Api.DTOs.Responses;

public class CommentResponse
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public int CreatedBy { get; set; }
    public string CreatedByName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
