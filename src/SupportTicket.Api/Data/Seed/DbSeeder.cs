using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SupportTicket.Api.Models;
using SupportTicket.Api.Models.Enums;

namespace SupportTicket.Api.Data.Seed;

public static class DbSeeder
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static async Task SeedAsync(AppDbContext db, IHostEnvironment env, CancellationToken cancellationToken = default)
    {
        if (await db.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        var seedFilePath = ResolveSeedDataPath(env);
        if (!File.Exists(seedFilePath))
        {
            throw new FileNotFoundException($"Seed data file not found: {seedFilePath}");
        }

        var json = await File.ReadAllTextAsync(seedFilePath, cancellationToken);
        var seedData = JsonSerializer.Deserialize<SeedDataFile>(json, JsonOptions)
            ?? throw new InvalidOperationException("Failed to deserialize seed data JSON.");

        foreach (var user in seedData.Users)
        {
            db.Users.Add(new User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            });
        }

        await db.SaveChangesAsync(cancellationToken);

        foreach (var ticket in seedData.Tickets)
        {
            db.Tickets.Add(new Ticket
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Priority = ticket.Priority,
                Status = ticket.Status,
                AssignedToId = ticket.AssignedToId,
                CreatedById = ticket.CreatedById,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            });
        }

        await db.SaveChangesAsync(cancellationToken);

        foreach (var comment in seedData.Comments)
        {
            db.Comments.Add(new Comment
            {
                Id = comment.Id,
                TicketId = comment.TicketId,
                Message = comment.Message,
                CreatedById = comment.CreatedById,
                CreatedAt = comment.CreatedAt
            });
        }

        await db.SaveChangesAsync(cancellationToken);
    }

    internal static string ResolveSeedDataPath(IHostEnvironment env)
    {
        var candidates = new[]
        {
            Path.GetFullPath(Path.Combine(env.ContentRootPath, "..", "..", "database", "seed-data", "seed-data.json")),
            Path.GetFullPath(Path.Combine(env.ContentRootPath, "..", "..", "..", "database", "seed-data", "seed-data.json")),
            Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database", "seed-data", "seed-data.json"))
        };

        foreach (var candidate in candidates)
        {
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        return candidates[0];
    }

    private sealed class SeedDataFile
    {
        public List<SeedUser> Users { get; set; } = [];
        public List<SeedTicket> Tickets { get; set; } = [];
        public List<SeedComment> Comments { get; set; } = [];
    }

    private sealed class SeedUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    private sealed class SeedTicket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TicketPriority Priority { get; set; }
        public TicketStatus Status { get; set; }
        public int? AssignedToId { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    private sealed class SeedComment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Message { get; set; } = string.Empty;
        public int CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
