using Codend.Application.Core.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Codend.Persistence;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConnectionString = configuration.GetConnectionString("database");
        services.AddDbContext<CodendApplicationDbContext>(options => options.UseSqlServer(databaseConnectionString));

        services.AddScoped<IUnitOfWork, CodendApplicationDbContext>();
        
        return services;
    }
}