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

    /// <summary>
    /// Creates new api error instance.
    /// </summary>
    /// <param name="errorCode">Error code.</param>
    /// <param name="message">Message.</param>
    public ApiError(string errorCode, string message)
    {
        ErrorCode = errorCode;
        Message = message;
        Metadata.Add("ErrorCode", errorCode);
    }

    /// <inheritdoc />
    public override string ToString() => new ReasonStringBuilder().WithReasonType(this.GetType())
        .WithInfo("ErrorCode", ErrorCode)
        .WithInfo("Message", Message)
        .Build();
}