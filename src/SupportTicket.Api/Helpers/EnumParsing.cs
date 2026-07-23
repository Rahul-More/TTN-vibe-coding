using SupportTicket.Api.Models.Enums;

namespace SupportTicket.Api.Helpers;

public static class EnumParsing
{
    public static bool TryParsePriority(string? value, out TicketPriority priority)
    {
        priority = default;
        return !string.IsNullOrWhiteSpace(value)
            && Enum.TryParse(value, ignoreCase: false, out priority)
            && Enum.IsDefined(priority);
    }

    public static bool TryParseStatus(string? value, out TicketStatus status)
    {
        status = default;
        return !string.IsNullOrWhiteSpace(value)
            && Enum.TryParse(value, ignoreCase: false, out status)
            && Enum.IsDefined(status);
    }

    public static string InvalidPriorityMessage(string value) =>
        $"Invalid priority value: {value}";

    public static string InvalidStatusMessage(string value) =>
        $"Invalid status value: {value}";
}
