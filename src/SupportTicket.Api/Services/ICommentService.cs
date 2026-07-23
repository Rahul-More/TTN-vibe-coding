using SupportTicket.Api.DTOs.Requests;
using SupportTicket.Api.DTOs.Responses;
using SupportTicket.Api.Services.Common;

namespace SupportTicket.Api.Services;

public interface ICommentService
{
    Task<ServiceResult<CommentResponse>> CreateAsync(int ticketId, CreateCommentRequest request);
}
