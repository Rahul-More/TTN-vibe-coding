using SupportTicket.Api.DTOs.Requests;
using SupportTicket.Api.DTOs.Responses;
using SupportTicket.Api.Services.Common;

namespace SupportTicket.Api.Services;

public interface ITicketService
{
    Task<ServiceResult<IReadOnlyList<TicketListItemResponse>>> ListAsync(string? search, string? statusFilter);
    Task<ServiceResult<TicketDetailResponse>> GetByIdAsync(int id);
    Task<ServiceResult<TicketDetailResponse>> CreateAsync(CreateTicketRequest request);
    Task<ServiceResult<TicketDetailResponse>> UpdateAsync(int id, UpdateTicketRequest request);
    Task<ServiceResult<TicketDetailResponse>> ChangeStatusAsync(int id, ChangeStatusRequest request);
}
