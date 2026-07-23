using System.Net;
using SupportTicket.Api.Models.Enums;
using SupportTicket.Api.Tests.Helpers;
using SupportTicket.Api.Tests.Infrastructure;
using Xunit;

namespace SupportTicket.Api.Tests.Integration;

[Collection("Integration")]
public class StatusTransitionIntegrationTests(CustomWebApplicationFactory factory)
{
    private readonly HttpClient _client = factory.Client;
    private readonly IServiceProvider _services = factory.Services;

    public static TheoryData<TicketStatus, TicketStatus, string[]> ValidTransitions =>
        new()
        {
            { TicketStatus.Open, TicketStatus.InProgress, ["Resolved", "Cancelled"] },
            { TicketStatus.InProgress, TicketStatus.Resolved, ["Closed"] },
            { TicketStatus.Resolved, TicketStatus.Closed, [] },
            { TicketStatus.Open, TicketStatus.Cancelled, [] },
            { TicketStatus.InProgress, TicketStatus.Cancelled, [] },
        };

    public static TheoryData<TicketStatus, TicketStatus> InvalidTransitions =>
        new()
        {
            { TicketStatus.Open, TicketStatus.Open },
            { TicketStatus.Open, TicketStatus.Closed },
            { TicketStatus.Open, TicketStatus.Resolved },
            { TicketStatus.Closed, TicketStatus.Open },
            { TicketStatus.Cancelled, TicketStatus.InProgress },
            { TicketStatus.Resolved, TicketStatus.Open },
        };

    [Theory]
    [MemberData(nameof(ValidTransitions))]
    public async Task PatchStatus_ValidTransition_Returns200AndUpdatesStatus(
        TicketStatus fromStatus,
        TicketStatus toStatus,
        string[] expectedValidNextStatuses)
    {
        var ticketId = await TicketApiHelpers.SeedTicketAtStatusAsync(_services, fromStatus);

        var response = await TicketApiHelpers.PatchStatusRawAsync(
            _client,
            ticketId,
            toStatus.ToString());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await TicketApiHelpers.ReadTicketDetailAsync(response);
        Assert.Equal(toStatus.ToString(), body.Status);
        Assert.Equal(expectedValidNextStatuses, body.ValidNextStatuses);

        var persisted = await TicketApiHelpers.GetTicketAsync(_client, ticketId);
        Assert.Equal(toStatus.ToString(), persisted.Status);
    }

    [Theory]
    [MemberData(nameof(InvalidTransitions))]
    public async Task PatchStatus_InvalidTransition_Returns400AndLeavesStatusUnchanged(
        TicketStatus fromStatus,
        TicketStatus toStatus)
    {
        var ticketId = await TicketApiHelpers.SeedTicketAtStatusAsync(_services, fromStatus);

        var response = await TicketApiHelpers.PatchStatusRawAsync(
            _client,
            ticketId,
            toStatus.ToString());

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var error = await TicketApiHelpers.ReadErrorAsync(response);
        Assert.Equal("INVALID_TRANSITION", error.Code);
        Assert.Equal($"Cannot transition from {fromStatus} to {toStatus}", error.Error);

        var persisted = await TicketApiHelpers.GetTicketAsync(_client, ticketId);
        Assert.Equal(fromStatus.ToString(), persisted.Status);
    }
}
