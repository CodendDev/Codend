using FluentResults;

namespace Codend.Application.Core.Abstractions.Authentication;

/// <summary>
/// Interface for external identity provider service to manage users.
/// </summary>
public interface IAuthService
{
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
    /// <param name="email">Application uniqe user email.</param>
    /// <param name="password">User password.</param>
    /// <param name="firstName">User first name</param>
    /// <param name="lastName">User last name.</param>
    /// <returns>JWT access token string or an error.</returns>
    public Task<Result<string>> RegisterAsync(string email, string password, string firstName, string lastName);
}