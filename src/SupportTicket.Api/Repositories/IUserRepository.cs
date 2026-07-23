using SupportTicket.Api.Models;

namespace SupportTicket.Api.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
}
