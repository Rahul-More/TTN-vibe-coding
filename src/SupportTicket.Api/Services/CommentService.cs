using SupportTicket.Api.DTOs.Requests;
using SupportTicket.Api.DTOs.Responses;
using SupportTicket.Api.Models;
using SupportTicket.Api.Repositories;
using SupportTicket.Api.Services.Common;

namespace SupportTicket.Api.Services;

public class CommentService : ICommentService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;

    public CommentService(
        ITicketRepository ticketRepository,
        ICommentRepository commentRepository,
        IUserRepository userRepository)
    {
        _ticketRepository = ticketRepository;
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }

    public async Task<ServiceResult<CommentResponse>> CreateAsync(int ticketId, CreateCommentRequest request)
    {
        var ticket = await _ticketRepository.GetByIdWithDetailsAsync(ticketId);
        if (ticket is null)
        {
            return ServiceResult<CommentResponse>.NotFound("Ticket not found");
        }

        var comment = new Comment
        {
            TicketId = ticketId,
            Message = request.Message.Trim(),
            CreatedById = request.CreatedBy,
            CreatedAt = DateTime.UtcNow
        };

        await _commentRepository.AddAsync(comment);

        var createdBy = await _userRepository.GetByIdAsync(request.CreatedBy);

        return ServiceResult<CommentResponse>.Ok(new CommentResponse
        {
            Id = comment.Id,
            Message = comment.Message,
            CreatedBy = comment.CreatedById,
            CreatedByName = createdBy!.Name,
            CreatedAt = comment.CreatedAt
        });
    }
}
