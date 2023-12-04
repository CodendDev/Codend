using Codend.Notifications.Email.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Codend.Notifications.Email;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddUserEmailNotifications(this IServiceCollection services)
    {
        services.AddScoped<EmailNotificationsService>();

        var assembly = typeof(DependencyInjection).Assembly;
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

        return services;
    }
}