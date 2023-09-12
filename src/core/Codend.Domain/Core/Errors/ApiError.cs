using FluentResults;

namespace Codend.Domain.Core.Errors;

/// <summary>
/// Base error class with error code and message.
/// </summary>
public class ApiError : Error
{
    /// <summary>
    /// Error code.
    /// </summary>
    public string ErrorCode { get; }

    /// <inheritdoc />
    public ApiError(string errorCode, string message)
    {
        ErrorCode = errorCode;
        Message = message;
    }
}