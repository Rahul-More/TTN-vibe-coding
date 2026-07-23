using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupportTicket.Api.DTOs.Requests;

public class CreateTicketRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Priority { get; set; } = string.Empty;
    public int? AssignedTo { get; set; }
    public int CreatedBy { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtensionData { get; set; }
}
