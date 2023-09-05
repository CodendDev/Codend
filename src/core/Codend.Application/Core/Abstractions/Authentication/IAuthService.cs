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
    /// <returns></returns>
    public Task<string> LoginAsync(string email, string password);
}