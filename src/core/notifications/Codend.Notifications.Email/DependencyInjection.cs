using Codend.Notifications.Email.Abstractions;
using Codend.Notifications.Email.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Codend.Notifications.Email;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddUserEmailNotifications(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (string.IsNullOrEmpty(configuration.GetConnectionString("AzureEmail")))
        {
            return services;
        }

        var assembly = typeof(DependencyInjection).Assembly;
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
        services.AddSingleton<IEmailService, AzureEmailService>();

        return services;
    }
}