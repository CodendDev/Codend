using Codend.Domain.Entities;

namespace Codend.Application.Core.Abstractions.Authentication;

/// <summary>
/// Interface to retrieve user id from HttpContext.
/// </summary>
public interface IHttpContextProvider
{
    /// <summary>
    /// Logged in user id.
    /// </summary>
    UserId UserId { get; }

    /// <summary>
    /// Returns user id extracted from token.
    /// </summary>
    /// <returns>User id or null if token/claim does not exits.</returns>
    public Guid GetUserGuid();
}