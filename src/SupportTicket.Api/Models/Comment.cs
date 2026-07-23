namespace SupportTicket.Api.Models;

public class Comment
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public string Message { get; set; } = string.Empty;
    public int CreatedById { get; set; }
    public DateTime CreatedAt { get; set; }

    public Ticket Ticket { get; set; } = null!;
    public User CreatedBy { get; set; } = null!;
}
