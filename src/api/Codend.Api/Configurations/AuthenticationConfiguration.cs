using System.Text;
using Codend.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Codend.Api.Configurations;

public static class AuthenticationConfiguration
{
    public static AuthenticationBuilder AddFusionauthAuthentication(this IServiceCollection services, IOptions<FusionauthConfiguration> configuration)
    {
        var fusionAuthConf = configuration.Value;
        
        return services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = fusionAuthConf.ApiUrl,
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