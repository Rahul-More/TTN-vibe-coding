using SupportTicket.Api.DTOs.Responses;

namespace SupportTicket.Api.Services;

public interface IUserService
{
    Task<IReadOnlyList<UserResponse>> GetAllAsync();
}
