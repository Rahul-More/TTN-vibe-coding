using SupportTicket.Api.Models;

namespace SupportTicket.Api.Repositories;

public interface ICommentRepository
{
    Task<Comment> AddAsync(Comment comment);
}
