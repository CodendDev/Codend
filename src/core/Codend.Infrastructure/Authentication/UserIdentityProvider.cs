using System.IdentityModel.Tokens.Jwt;
using Codend.Application.Core.Abstractions.Authentication;
using Codend.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Codend.Infrastructure.Authentication;

/// <summary>
/// Implementation of IUserIdentityProvider which uses JWT token to
/// obtain required informations e.g. GetUserId uses "sub" claim to retrieve id.
/// </summary>
public class UserIdentityProvider : IUserIdentityProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserIdentityProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    //Todo:replace all UserId refrences with GetUserId method.
    public UserId UserId => new UserId((Guid)GetUserId()!);
    
    /// <inheritdoc />
    public Guid? GetUserId()
    {
        var jwtToken = _contextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (jwtToken == null)
        {
            // User is not authenticated
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(jwtToken);

        if (token == null || token.Claims.All(c => c.Type != "sub"))
        {
            // JWT token is invalid
            return null;
        }

        var userId = Guid.Parse(token.Claims.First(c => c.Type == "sub").Value);
        return userId;
    }
    
}