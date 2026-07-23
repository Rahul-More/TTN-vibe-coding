namespace SupportTicket.Api.Services.Common;

public sealed class ServiceResult<T>
{
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public string? Error { get; init; }
    public string? Code { get; init; }
    public bool IsNotFound { get; init; }

    public static ServiceResult<T> Ok(T value) =>
        new() { IsSuccess = true, Value = value };

    public static ServiceResult<T> Fail(string error, string? code = null) =>
        new() { IsSuccess = false, Error = error, Code = code };

    public static ServiceResult<T> NotFound(string error) =>
        new() { IsSuccess = false, IsNotFound = true, Error = error };
}
