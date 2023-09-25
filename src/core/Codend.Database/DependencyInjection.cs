using Codend.Application.Core.Abstractions.Data;
using Codend.Domain.Repositories;
using Codend.Persistence;
using Codend.Persistence.Postgres;
using Codend.Persistence.Repositories;
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
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositories();

        // Add Postgres
        var postgresConnectionString = configuration.GetConnectionString("PostgresDatabase");
        if (!string.IsNullOrEmpty(postgresConnectionString))
        {
            services.AddCodendDbContext<PostgresCodendDbContext>(
                options => options.UseNpgsql(postgresConnectionString));
            return services;
        }

        // Add SqlServer
        var sqlServerConnectionString = configuration.GetConnectionString("SqlServerDatabase");
        if (!string.IsNullOrEmpty(sqlServerConnectionString))
        {
            services.AddCodendDbContext<SqlServerCodendDbContext>(
                options => options.UseSqlServer(sqlServerConnectionString));
            return services;
        }

        throw new NullReferenceException("Database connection string can't be null.");
    }

    private static IServiceCollection AddCodendDbContext<T>(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> optionsAction)
        where T : CodendApplicationDbContext, IUnitOfWork, IMigratable
    {
        services.AddDbContext<CodendApplicationDbContext, T>(optionsAction);

        services.AddScoped<IUnitOfWork, T>(serviceProvider => serviceProvider.GetRequiredService<T>()); // Save changes
        services.AddScoped<IQueryableSets, T>(); // Entities DbSets 
        services.AddScoped<IMigratable, T>(); // Migrations at startup

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
        services.AddScoped<IProjectTaskStatusRepository, ProjectTaskStatusRepository>();
        services.AddScoped<IStoryRepository, StoryRepository>();
        services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
        services.AddScoped<IEpicRepository, EpicRepository>();

        return services;
    }
}