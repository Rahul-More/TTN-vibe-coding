using System.Net;
using SupportTicket.Api.Models.Enums;
using SupportTicket.Api.Tests.Helpers;
using SupportTicket.Api.Tests.Infrastructure;
using Xunit;

namespace SupportTicket.Api.Tests.Integration;

[Collection("Integration")]
public class SearchFilterIntegrationTests(CustomWebApplicationFactory factory)
{
    private readonly HttpClient _client = factory.Client;
    private readonly IServiceProvider _services = factory.Services;

    [Fact]
    public async Task GetTickets_WithSearchAndStatusFilter_ReturnsMatchingTickets()
    {
        await TicketApiHelpers.SeedTicketAsync(
            _services,
            "Password RESET required",
            "User cannot log in",
            TicketStatus.Open);
        await TicketApiHelpers.SeedTicketAsync(
            _services,
            "Billing issue",
            "Needs password reset follow-up",
            TicketStatus.InProgress);
        await TicketApiHelpers.SeedTicketAsync(
            _services,
            "Password RESET completed",
            "Resolved earlier",
            TicketStatus.Resolved);

        var response = await TicketApiHelpers.GetTicketsListRawAsync(
            _client,
            search: "reset",
            status: "Open");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var tickets = await TicketApiHelpers.ReadTicketListAsync(response);
        Assert.Single(tickets);
        Assert.Equal("Password RESET required", tickets[0].Title);
        Assert.Equal(TicketStatus.Open.ToString(), tickets[0].Status);
    }

    [Fact]
    public async Task GetTickets_WithNoMatches_Returns200AndEmptyArray()
    {
        await TicketApiHelpers.SeedTicketAsync(
            _services,
            "Unrelated ticket",
            "No matching keywords",
            TicketStatus.Open);

        var response = await TicketApiHelpers.GetTicketsListRawAsync(
            _client,
            search: "nonexistent-keyword-xyz");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var tickets = await TicketApiHelpers.ReadTicketListAsync(response);
        Assert.Empty(tickets);
    }
}
