namespace Codend.Application.Exceptions;

/// <summary>
/// Represents an exception that occurs when request in controller is null.
/// </summary>
public sealed class InvalidRequestException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidRequestException"/> class.
    /// </summary>
    public InvalidRequestException()
        : base("Request was null.")
    {
    }
}