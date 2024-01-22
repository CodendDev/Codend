using Codend.Persistence.Postgres;
using Codend.Persistence.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Codend.Database;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Postgres
        var postgresConnectionString = configuration.GetConnectionString("PostgresDatabase");
        if (!string.IsNullOrEmpty(postgresConnectionString))
        {
            return PostgresCodendDbContext.AddDatabase(services, postgresConnectionString);
        }

        // Add SqlServer
        var sqlServerConnectionString = configuration.GetConnectionString("SqlServerDatabase");
        if (!string.IsNullOrEmpty(sqlServerConnectionString))
        {
            return SqlServerCodendDbContext.AddDatabase(services, sqlServerConnectionString);
        }

        throw new NullReferenceException("Database connection string can't be null.");
    }
}