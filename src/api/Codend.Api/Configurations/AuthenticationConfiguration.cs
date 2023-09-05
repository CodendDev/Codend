using System.Text;
using Codend.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Codend.Api.Configurations;

/// <summary>
/// Authentication configuration for fusionauth.
/// </summary>
public static class AuthenticationConfiguration
{
    public static AuthenticationBuilder AddFusionauthAuthentication(this IServiceCollection services)
    {
        var fusionAuthConf = services.BuildServiceProvider().GetService<IOptions<FusionauthConfiguration>>()!.Value;
        
        return services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = fusionAuthConf.Issuer,
                ValidAudience = fusionAuthConf.ApplicationId,
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(fusionAuthConf.SigningKey)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
    }
}