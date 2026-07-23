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
        TicketStatus status) =>
        await SeedTicketAsync(
            services,
            "Integration test ticket",
            "Created for status transition tests",
            status);

    public static async Task<int> SeedTicketAsync(
        IServiceProvider services,
        string title,
        string? description,
        TicketStatus status,
        TicketPriority priority = TicketPriority.Medium)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var now = DateTime.UtcNow;

        var ticket = new Ticket
        {
            Title = title,
            Description = description,
            Priority = priority,
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

    public static async Task<HttpResponseMessage> PostTicketRawAsync(
        HttpClient client,
        object body)
    {
        return await client.PostAsJsonAsync("/api/tickets", body);
    }

    public static async Task<HttpResponseMessage> PostCommentRawAsync(
        HttpClient client,
        int ticketId,
        object body)
    {
        return await client.PostAsJsonAsync($"/api/tickets/{ticketId}/comments", body);
    }

    public static async Task<HttpResponseMessage> GetTicketRawAsync(HttpClient client, int ticketId)
    {
        return await client.GetAsync($"/api/tickets/{ticketId}");
    }

    public static async Task<HttpResponseMessage> PutTicketRawAsync(
        HttpClient client,
        int ticketId,
        object body)
    {
        return await client.PutAsJsonAsync($"/api/tickets/{ticketId}", body);
    }

    public static async Task<HttpResponseMessage> GetTicketsListRawAsync(
        HttpClient client,
        string? search = null,
        string? status = null)
    {
        var query = new List<string>();
        if (search is not null)
        {
            query.Add($"search={Uri.EscapeDataString(search)}");
        }

        if (status is not null)
        {
            query.Add($"status={Uri.EscapeDataString(status)}");
        }

        var path = query.Count == 0
            ? "/api/tickets"
            : $"/api/tickets?{string.Join("&", query)}";

        return await client.GetAsync(path);
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

    public static async Task<IReadOnlyList<TicketListItemResponse>> ReadTicketListAsync(
        HttpResponseMessage response)
    {
        return (await response.Content.ReadFromJsonAsync<List<TicketListItemResponse>>(JsonOptions))!;
    }
}
