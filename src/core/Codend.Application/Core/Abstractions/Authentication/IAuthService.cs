using FluentResults;

namespace Codend.Application.Core.Abstractions.Authentication;

/// <summary>
/// Interface for external identity provider service to manage users.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Minimum password length.
    /// </summary>
    public const int MinPasswordLength = 8;

    /// <summary>
    /// Maximum password length.
    /// </summary>
    public const int MaxPasswordLength = 32;

    /// <summary>
    /// Maximum email length.
    /// </summary>
    public const int MaxEmailLength = 320;

    /// <summary>
    /// Maximum first name length.
    /// </summary>
    public const int MaxFirstNameLength = 32;

    /// <summary>
    /// Maximum last name length.
    /// </summary>
    public const int MaxLastNameLength = 64;

    /// <summary>
    /// Login user with given credentials.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns>JWT access token string or an error.</returns>
    public Task<Result<string>> LoginAsync(string email, string password);

    /// <summary>
    /// Create new user with provided data.
    /// </summary>
    /// <param name="email">Application unique user email.</param>
    /// <param name="password">User password.</param>
    /// <param name="firstName">User first name</param>
    /// <param name="lastName">User last name.</param>
    /// <returns>JWT access token string or an error.</returns>
    public Task<Result<string>> RegisterAsync(string email, string password, string firstName, string lastName);
}