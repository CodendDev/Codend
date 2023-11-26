using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Common;
using Codend.Application.Core.Abstractions.Notifications;
using Codend.Application.Core.Abstractions.Services;
using Codend.Infrastructure.Authentication;
using Codend.Infrastructure.Common;
using Codend.Infrastructure.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Codend.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTime, MachineDateTime>();

        services.AddScoped<IHttpContextProvider, HttpContextProvider>();
        services.AddScoped<IAuthService, FusionAuthService>();
        services.AddScoped<IUserService, FusionAuthService>();

        services.AddScoped<IUserNotificationService, ExampleNotificationService>();

        return services;
    }
}