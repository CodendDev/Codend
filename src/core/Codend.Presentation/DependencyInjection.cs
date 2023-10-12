using Codend.Presentation.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using static Codend.Presentation.Infrastructure.Authorization.ProjectOperations;

namespace Codend.Presentation;

/// <summary>
/// Dependency injection class.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(IsProjectMemberPolicy,
                policy => policy.AddRequirements(Member));
            options.AddPolicy(IsProjectOwnerPolicy,
                policy => policy.AddRequirements(Owner));
        });
        services.AddScoped<IAuthorizationHandler, ProjectOperationsAuthorizationHandler>();
        
        return services;
    }
}