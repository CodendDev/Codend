using Codend.Domain.Entities;

namespace Codend.Application.Core.Abstractions.Authentication;

/// <summary>
/// Interface to retrieve user id and other information from HttpContext.
/// </summary>
public interface IHttpContextProvider
{
    /// <summary>
    /// Logged in user id.
    /// </summary>
    UserId UserId { get; }
    
    /// <summary>
    /// ProjectId from route if exists.
    /// </summary>
    ProjectId? ProjectId { get; }

    /// <summary>
    /// Sets response status code.
    /// </summary>
    public void SetResponseStatusCode(int statusCode);
}