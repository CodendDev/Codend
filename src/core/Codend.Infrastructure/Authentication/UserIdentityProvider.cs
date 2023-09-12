using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using Codend.Application.Core.Abstractions.Authentication;
using Codend.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Codend.Infrastructure.Authentication;

/// <summary>
/// Implementation of IUserIdentityProvider which uses JWT token to
/// obtain required information e.g. GetUserId uses "sub" claim to retrieve id.
/// </summary>
public class UserIdentityProvider : IUserIdentityProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserIdentityProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public UserId UserId => new(GetUserGuid());

    /// <inheritdoc />
    public Guid GetUserGuid()
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
}