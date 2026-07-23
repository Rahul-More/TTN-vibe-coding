using SupportTicket.Api.Models;
using SupportTicket.Api.Models.Enums;

namespace SupportTicket.Api.Repositories;

public interface ITicketRepository
{
    Task<IReadOnlyList<Ticket>> ListAsync(string? search, TicketStatus? status);
    Task<Ticket?> GetByIdWithDetailsAsync(int id);
    Task<Ticket> AddAsync(Ticket ticket);
    Task UpdateAsync(Ticket ticket);
}
