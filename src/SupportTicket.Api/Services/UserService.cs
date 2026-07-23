using SupportTicket.Api.DTOs.Responses;
using SupportTicket.Api.Repositories;

namespace SupportTicket.Api.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<UserResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserResponse
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role
        }).ToList();
    }
}
