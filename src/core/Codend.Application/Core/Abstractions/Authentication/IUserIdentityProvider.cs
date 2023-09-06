using Codend.Domain.Entities;

namespace Codend.Application.Core.Abstractions.Authentication;

/// <summary>
/// Interface to retrive user id from HttpContext.
/// </summary>
public interface IUserIdentityProvider
{
    UserId UserId { get; }

    /// <summary>
    /// Returns user id extraced from token.
    /// </summary>
    /// <returns>User id or null if token/claim does not exits.</returns>
    public Guid? GetUserId();
}