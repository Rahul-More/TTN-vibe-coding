using Microsoft.EntityFrameworkCore;
using SupportTicket.Api.Data;
using SupportTicket.Api.Models;
using SupportTicket.Api.Models.Enums;

namespace SupportTicket.Api.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _db;

    public TicketRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Ticket>> ListAsync(string? search, TicketStatus? status)
    {
        var query = _db.Tickets
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = $"%{search.Trim()}%";
            query = query.Where(t =>
                EF.Functions.ILike(t.Title, pattern) ||
                (t.Description != null && EF.Functions.ILike(t.Description, pattern)));
        }

        return await query
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<Ticket?> GetByIdWithDetailsAsync(int id) =>
        await _db.Tickets
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .Include(t => t.Comments.OrderBy(c => c.CreatedAt))
                .ThenInclude(c => c.CreatedBy)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<Ticket> AddAsync(Ticket ticket)
    {
        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync();
        return ticket;
    }

    public async Task UpdateAsync(Ticket ticket)
    {
        _db.Tickets.Update(ticket);
        await _db.SaveChangesAsync();
    }
}
