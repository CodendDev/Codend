namespace Codend.Application.Exceptions;

/// <summary>
/// Represents an exception that occurs when authentication service fails.
/// </summary>
public sealed class AuthenticationServiceException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationServiceException"/> class.
    /// </summary>
    public AuthenticationServiceException(string? message)
        : base(message)
    {
    }
}