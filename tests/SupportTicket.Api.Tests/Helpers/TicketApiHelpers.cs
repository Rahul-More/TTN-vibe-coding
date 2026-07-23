using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using SupportTicket.Api.Data;
using SupportTicket.Api.DTOs.Responses;
using SupportTicket.Api.Models;
using SupportTicket.Api.Models.Enums;

namespace SupportTicket.Api.Tests.Helpers;

public static class TicketApiHelpers
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<int> SeedTicketAtStatusAsync(
        IServiceProvider services,
        TicketStatus status)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var now = DateTime.UtcNow;

        var ticket = new Ticket
        {
            Title = "Integration test ticket",
            Description = "Created for status transition tests",
            Priority = TicketPriority.Medium,
            Status = status,
            CreatedById = 1,
            CreatedAt = now,
            UpdatedAt = now
        };

        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();
        return ticket.Id;
    }

    public static async Task<HttpResponseMessage> PatchStatusRawAsync(
        HttpClient client,
        int ticketId,
        string status)
    {
        return await client.PatchAsJsonAsync(
            $"/api/tickets/{ticketId}/status",
            new { status });
    }

    public static async Task<TicketDetailResponse> GetTicketAsync(HttpClient client, int ticketId)
    {
        var response = await client.GetAsync($"/api/tickets/{ticketId}");
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<TicketDetailResponse>(JsonOptions))!;
    }

    public static async Task<ErrorResponse> ReadErrorAsync(HttpResponseMessage response)
    {
        return (await response.Content.ReadFromJsonAsync<ErrorResponse>(JsonOptions))!;
    }

    public static async Task<TicketDetailResponse> ReadTicketDetailAsync(HttpResponseMessage response)
    {
        return (await response.Content.ReadFromJsonAsync<TicketDetailResponse>(JsonOptions))!;
    }
}
