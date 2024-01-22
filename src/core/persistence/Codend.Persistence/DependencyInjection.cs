using Codend.Application.Core.Abstractions.Data;
using Codend.Domain.Repositories;
using Codend.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Codend.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase<T>(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> optionsAction
    )
        where T : CodendApplicationDbContext
    {
        services.AddDbContext<CodendApplicationDbContext, T>(optionsAction);

        services.AddScoped<IUnitOfWork, T>(serviceProvider => serviceProvider.GetRequiredService<T>()); // Save changes
        services.AddScoped<IQueryableSets, T>(); // Entities DbSets 
        services.AddScoped<IMigratable, T>(); // Migrations at startup

        // repositories
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
        services.AddScoped<IProjectTaskStatusRepository, ProjectTaskStatusRepository>();
        services.AddScoped<IStoryRepository, StoryRepository>();
        services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
        services.AddScoped<IEpicRepository, EpicRepository>();
        services.AddScoped<ISprintRepository, SprintRepository>();
        services.AddScoped<ISprintProjectTaskRepository, SprintProjectTaskRepository>();

        return services;
    }
}