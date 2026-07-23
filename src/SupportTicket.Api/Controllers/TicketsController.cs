using Microsoft.AspNetCore.Mvc;
using SupportTicket.Api.DTOs.Requests;
using SupportTicket.Api.DTOs.Responses;
using SupportTicket.Api.Services;

namespace SupportTicket.Api.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly ICommentService _commentService;

    public TicketsController(ITicketService ticketService, ICommentService commentService)
    {
        _ticketService = ticketService;
        _commentService = commentService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TicketListItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<TicketListItemResponse>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? status)
    {
        var result = await _ticketService.ListAsync(search, status);
        return ApiResult.From(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TicketDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketDetailResponse>> GetById(int id)
    {
        var result = await _ticketService.GetByIdAsync(id);
        return ApiResult.From(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TicketDetailResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TicketDetailResponse>> Create([FromBody] CreateTicketRequest request)
    {
        var result = await _ticketService.CreateAsync(request);
        if (!result.IsSuccess)
        {
            return ApiResult.From(result);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TicketDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketDetailResponse>> Update(int id, [FromBody] UpdateTicketRequest request)
    {
        var result = await _ticketService.UpdateAsync(id, request);
        return ApiResult.From(result);
    }

    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(typeof(TicketDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketDetailResponse>> ChangeStatus(int id, [FromBody] ChangeStatusRequest request)
    {
        var result = await _ticketService.ChangeStatusAsync(id, request);
        return ApiResult.From(result);
    }

    [HttpPost("{ticketId:int}/comments")]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CommentResponse>> AddComment(int ticketId, [FromBody] CreateCommentRequest request)
    {
        var result = await _commentService.CreateAsync(ticketId, request);
        if (!result.IsSuccess)
        {
            return ApiResult.From(result);
        }

        return CreatedAtAction(nameof(GetById), new { id = ticketId }, result.Value);
    }
}
