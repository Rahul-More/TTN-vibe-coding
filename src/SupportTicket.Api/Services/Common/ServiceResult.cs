namespace SupportTicket.Api.Services.Common;

public sealed class ServiceResult
{
    public bool IsSuccess { get; init; }
    public string? Error { get; init; }
    public string? Code { get; init; }

    public static ServiceResult Ok() => new() { IsSuccess = true };

    public static ServiceResult Fail(string error, string? code = null) =>
        new() { IsSuccess = false, Error = error, Code = code };
}
