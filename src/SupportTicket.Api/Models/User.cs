namespace SupportTicket.Api.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public ICollection<Ticket> TicketsCreated { get; set; } = new List<Ticket>();
    public ICollection<Ticket> TicketsAssigned { get; set; } = new List<Ticket>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
