using System.Net;
using SupportTicket.Api.Models.Enums;
using SupportTicket.Api.Tests.Helpers;
using SupportTicket.Api.Tests.Infrastructure;
using Xunit;

namespace SupportTicket.Api.Tests.Integration;

[Collection("Integration")]
public class ValidationErrorIntegrationTests(CustomWebApplicationFactory factory)
{
    private readonly HttpClient _client = factory.Client;
    private readonly IServiceProvider _services = factory.Services;

    [Fact]
    public async Task PostTicket_EmptyTitle_Returns400()
    {
        var response = await TicketApiHelpers.PostTicketRawAsync(_client, new
        {
            title = "",
            description = "Test description",
            priority = "Medium",
            createdBy = 1
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var error = await TicketApiHelpers.ReadErrorAsync(response);
        Assert.Equal("Title is required", error.Error);
        Assert.Null(error.Code);
    }

    [Fact]
    public async Task PostTicket_InvalidCreatedBy_Returns400()
    {
        var response = await TicketApiHelpers.PostTicketRawAsync(_client, new
        {
            title = "Valid title",
            description = "Test description",
            priority = "Medium",
            createdBy = 99999
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var error = await TicketApiHelpers.ReadErrorAsync(response);
        Assert.Equal("User with id 99999 does not exist", error.Error);
        Assert.Null(error.Code);
    }

    [Fact]
    public async Task GetTicket_NonExistentId_Returns404()
    {
        var response = await TicketApiHelpers.GetTicketRawAsync(_client, 999999);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var error = await TicketApiHelpers.ReadErrorAsync(response);
        Assert.Equal("Ticket not found", error.Error);
        Assert.Null(error.Code);
    }

    [Fact]
    public async Task PostComment_NonExistentTicket_Returns404()
    {
        var response = await TicketApiHelpers.PostCommentRawAsync(_client, 999999, new
        {
            message = "Test comment",
            createdBy = 1
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var error = await TicketApiHelpers.ReadErrorAsync(response);
        Assert.Equal("Ticket not found", error.Error);
        Assert.Null(error.Code);
    }

    [Fact]
    public async Task PostComment_EmptyMessage_Returns400()
    {
        var ticketId = await TicketApiHelpers.SeedTicketAtStatusAsync(_services, TicketStatus.Open);

        var response = await TicketApiHelpers.PostCommentRawAsync(_client, ticketId, new
        {
            message = "",
            createdBy = 1
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var error = await TicketApiHelpers.ReadErrorAsync(response);
        Assert.Equal("Message is required", error.Error);
        Assert.Null(error.Code);
    }
}
