using Codend.Application.Core.Abstractions.Data;
using Codend.Persistence.Postgres;
using Codend.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
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
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>The same service collection.</returns>
    public static Type AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Postgres
        var postgresConnectionString = configuration.GetConnectionString("PostgresDatabase");
        if (!string.IsNullOrEmpty(postgresConnectionString))
        {
            services.AddCodendDbContext<PostgresCodendDbContext>(
                options => options.UseNpgsql(postgresConnectionString));
            return typeof(PostgresCodendDbContext);
            // return services;
        }

        // Add SqlServer
        var sqlServerConnectionString = configuration.GetConnectionString("Database");
        if (!string.IsNullOrEmpty(sqlServerConnectionString))
        {
            services.AddCodendDbContext<SqlServerCodendDbContext>(
                options => options.UseSqlServer(sqlServerConnectionString));
            return typeof(SqlServerCodendDbContext);
            // return services;
        }

        throw new NullReferenceException("Database connection string can't be null.");
    }

    private static IServiceCollection AddCodendDbContext<T>(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> optionsAction)
        where T : DbContext, IUnitOfWork
    {
        services.AddDbContext<T>(optionsAction);

        services.AddScoped<IUnitOfWork, T>();

        return services;
    }
}