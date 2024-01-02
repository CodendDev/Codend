using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using Codend.Application.Core.Abstractions.Authentication;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Codend.Infrastructure.Authentication;

/// <summary>
/// Implementation of IHttpContextProvider which uses JWT token to
/// obtain required information e.g. GetUserId uses "sub" claim to retrieve id.
/// </summary>
public class HttpContextProvider : IHttpContextProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpContextProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public UserId UserId => GetUserGuid().GuidConversion<UserId>();

    public ProjectId? ProjectId => GetProjectIdFromRoute().GuidConversion<ProjectId>();
    
    private Guid GetUserGuid()
    {
        var jwtToken = _contextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ")
            .Last();
        if (jwtToken == null)
        {
            // User is not authenticated
            throw new AuthenticationException("User is not authenticated, but should be.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(jwtToken);

        if (token == null || token.Claims.All(c => c.Type != "sub"))
        {
            // JWT token is invalid
            throw new AuthenticationException("Invalid token - no 'sub' claim with user id.");
        }

        var userId = Guid.Parse(token.Claims.First(c => c.Type == "sub").Value);
        return userId;
    }

    private Guid? GetProjectIdFromRoute()
    {
        var projectIdString = _contextAccessor.HttpContext?.GetRouteData().Values["projectId"]?.ToString();
        if (projectIdString is null || !Guid.TryParse(projectIdString, out var projectId))
        {
            return null;
        }

        return projectId;
    }
    
    public void SetResponseStatusCode(int statusCode)
    {
        if (_contextAccessor.HttpContext is null)
        {
            return;
        }
        _contextAccessor.HttpContext!.Response.StatusCode = statusCode;
    }
}