using System.Net;
using SupportTicket.Api.Models.Enums;
using SupportTicket.Api.Tests.Helpers;
using SupportTicket.Api.Tests.Infrastructure;
using Xunit;

namespace SupportTicket.Api.Tests.Integration;

[Collection("Integration")]
public class StatusIsolationIntegrationTests(CustomWebApplicationFactory factory)
{
    private readonly HttpClient _client = factory.Client;
    private readonly IServiceProvider _services = factory.Services;

    [Fact]
    public async Task PostTicket_WithStatusInBody_Returns400()
    {
        var response = await TicketApiHelpers.PostTicketRawAsync(_client, new
        {
            title = "Valid title",
            description = "Test description",
            priority = "Medium",
            createdBy = 1,
            status = "InProgress"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var error = await TicketApiHelpers.ReadErrorAsync(response);
        Assert.Equal("Status cannot be set on create. Use PATCH /api/tickets/{id}/status.", error.Error);
        Assert.Null(error.Code);
    }

    [Fact]
    public async Task PutTicket_WithStatusInBody_Returns400()
    {
        var ticketId = await TicketApiHelpers.SeedTicketAtStatusAsync(_services, TicketStatus.Open);

        var response = await TicketApiHelpers.PutTicketRawAsync(_client, ticketId, new
        {
            title = "Updated title",
            description = "Updated description",
            priority = "Medium",
            status = "InProgress"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var error = await TicketApiHelpers.ReadErrorAsync(response);
        Assert.Equal("Status cannot be updated via PUT. Use PATCH /api/tickets/{id}/status.", error.Error);
        Assert.Null(error.Code);
    }

    [Fact]
    public async Task PatchStatus_InvalidStatusEnum_Returns400WithoutInvalidTransitionCode()
    {
        var ticketId = await TicketApiHelpers.SeedTicketAtStatusAsync(_services, TicketStatus.Open);

        var response = await TicketApiHelpers.PatchStatusRawAsync(_client, ticketId, "Urgent");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var error = await TicketApiHelpers.ReadErrorAsync(response);
        Assert.Equal("Invalid status value: Urgent", error.Error);
        Assert.Null(error.Code);

        var persisted = await TicketApiHelpers.GetTicketAsync(_client, ticketId);
        Assert.Equal(TicketStatus.Open.ToString(), persisted.Status);
    }
}
