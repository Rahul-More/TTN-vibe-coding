using System.Text.Json.Serialization;

namespace SupportTicket.Api.DTOs.Responses;

public record ErrorResponse(
    string Error,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Code = null);
