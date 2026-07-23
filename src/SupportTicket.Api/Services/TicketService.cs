using SupportTicket.Api.DTOs.Requests;
using SupportTicket.Api.DTOs.Responses;
using SupportTicket.Api.Helpers;
using SupportTicket.Api.Models;
using SupportTicket.Api.Models.Enums;
using SupportTicket.Api.Repositories;
using SupportTicket.Api.Services.Common;

namespace SupportTicket.Api.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IStatusTransitionService _statusTransitionService;

    public TicketService(
        ITicketRepository ticketRepository,
        IStatusTransitionService statusTransitionService)
    {
        _ticketRepository = ticketRepository;
        _statusTransitionService = statusTransitionService;
    }

    public async Task<ServiceResult<IReadOnlyList<TicketListItemResponse>>> ListAsync(
        string? search,
        string? statusFilter)
    {
        TicketStatus? status = null;

        if (!string.IsNullOrWhiteSpace(statusFilter))
        {
            if (!EnumParsing.TryParseStatus(statusFilter, out var parsedStatus))
            {
                return ServiceResult<IReadOnlyList<TicketListItemResponse>>.Fail(
                    EnumParsing.InvalidStatusMessage(statusFilter));
            }

            status = parsedStatus;
        }

        var tickets = await _ticketRepository.ListAsync(search, status);
        var response = tickets.Select(MapToListItem).ToList();
        return ServiceResult<IReadOnlyList<TicketListItemResponse>>.Ok(response);
    }

    public async Task<ServiceResult<TicketDetailResponse>> GetByIdAsync(int id)
    {
        var ticket = await _ticketRepository.GetByIdWithDetailsAsync(id);
        if (ticket is null)
        {
            return ServiceResult<TicketDetailResponse>.NotFound("Ticket not found");
        }

        return ServiceResult<TicketDetailResponse>.Ok(MapToDetail(ticket));
    }

    public async Task<ServiceResult<TicketDetailResponse>> CreateAsync(CreateTicketRequest request)
    {
        EnumParsing.TryParsePriority(request.Priority, out var priority);

        var now = DateTime.UtcNow;
        var ticket = new Ticket
        {
            Title = request.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            Priority = priority,
            Status = TicketStatus.Open,
            AssignedToId = request.AssignedTo,
            CreatedById = request.CreatedBy,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _ticketRepository.AddAsync(ticket);

        var created = await _ticketRepository.GetByIdWithDetailsAsync(ticket.Id);
        return ServiceResult<TicketDetailResponse>.Ok(MapToDetail(created!));
    }

    public async Task<ServiceResult<TicketDetailResponse>> UpdateAsync(int id, UpdateTicketRequest request)
    {
        var ticket = await _ticketRepository.GetByIdWithDetailsAsync(id);
        if (ticket is null)
        {
            return ServiceResult<TicketDetailResponse>.NotFound("Ticket not found");
        }

        EnumParsing.TryParsePriority(request.Priority, out var priority);

        ticket.Title = request.Title.Trim();
        ticket.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        ticket.Priority = priority;
        ticket.AssignedToId = request.AssignedTo;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _ticketRepository.UpdateAsync(ticket);

        var updated = await _ticketRepository.GetByIdWithDetailsAsync(id);
        return ServiceResult<TicketDetailResponse>.Ok(MapToDetail(updated!));
    }

    public async Task<ServiceResult<TicketDetailResponse>> ChangeStatusAsync(int id, ChangeStatusRequest request)
    {
        var ticket = await _ticketRepository.GetByIdWithDetailsAsync(id);
        if (ticket is null)
        {
            return ServiceResult<TicketDetailResponse>.NotFound("Ticket not found");
        }

        if (!EnumParsing.TryParseStatus(request.Status, out var newStatus))
        {
            return ServiceResult<TicketDetailResponse>.Fail(
                EnumParsing.InvalidStatusMessage(request.Status));
        }

        var validation = _statusTransitionService.ValidateTransition(ticket.Status, newStatus);
        if (!validation.IsSuccess)
        {
            return ServiceResult<TicketDetailResponse>.Fail(validation.Error!, validation.Code);
        }

        ticket.Status = newStatus;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _ticketRepository.UpdateAsync(ticket);

        var updated = await _ticketRepository.GetByIdWithDetailsAsync(id);
        return ServiceResult<TicketDetailResponse>.Ok(MapToDetail(updated!));
    }

    private TicketDetailResponse MapToDetail(Ticket ticket)
    {
        var detail = new TicketDetailResponse
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            Priority = ticket.Priority.ToString(),
            Status = ticket.Status.ToString(),
            AssignedTo = ticket.AssignedToId,
            AssignedToName = ticket.AssignedTo?.Name,
            CreatedBy = ticket.CreatedById,
            CreatedByName = ticket.CreatedBy.Name,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            ValidNextStatuses = _statusTransitionService.GetValidNextStatuses(ticket.Status).ToList(),
            Comments = ticket.Comments.Select(c => new CommentResponse
            {
                Id = c.Id,
                Message = c.Message,
                CreatedBy = c.CreatedById,
                CreatedByName = c.CreatedBy.Name,
                CreatedAt = c.CreatedAt
            }).ToList()
        };

        return detail;
    }

    private static TicketListItemResponse MapToListItem(Ticket ticket) =>
        new()
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            Priority = ticket.Priority.ToString(),
            Status = ticket.Status.ToString(),
            AssignedTo = ticket.AssignedToId,
            AssignedToName = ticket.AssignedTo?.Name,
            CreatedBy = ticket.CreatedById,
            CreatedByName = ticket.CreatedBy.Name,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt
        };
}
