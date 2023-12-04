using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Common;
using Codend.Application.Core.Abstractions.Services;
using Codend.Infrastructure.Authentication;
using Codend.Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Codend.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IDateTime, MachineDateTime>();

        services.AddScoped<IHttpContextProvider, HttpContextProvider>();
        services.AddScoped<IAuthService, FusionAuthService>();
        services.AddScoped<IUserService, FusionAuthService>();

        return services;
    }
}