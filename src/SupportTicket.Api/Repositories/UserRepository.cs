using Microsoft.EntityFrameworkCore;
using SupportTicket.Api.Data;
using SupportTicket.Api.Models;

namespace SupportTicket.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync() =>
        await _db.Users.OrderBy(u => u.Id).ToListAsync();

    public async Task<User?> GetByIdAsync(int id) =>
        await _db.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<bool> ExistsAsync(int id) =>
        await _db.Users.AnyAsync(u => u.Id == id);
}
