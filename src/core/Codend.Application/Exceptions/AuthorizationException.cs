namespace Codend.Application.Exceptions;

/// <summary>
/// Represents an exception that occurs when authorization failed or invalid.
/// </summary>
public sealed class AuthorizationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationException"/> class.
    /// </summary>
    public AuthorizationException()
        : base("Unauthorized access or invalid project.")
    {
    }
}