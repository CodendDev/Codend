using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Codend.Persistence.Repositories;

public class ProjectRepository : GenericRepository<ProjectId, Guid, Project>, IProjectRepository
{
    public ProjectRepository(CodendApplicationDbContext context) : base(context)
    {
    }

    public Task<bool> ProjectContainsEpic(ProjectId projectId, EpicId epicId)
    {
        var contains = Context.Set<Epic>()
            .AnyAsync(e => e.Id == epicId && e.ProjectId == projectId);

        return contains;
    }

    public async Task<int> CountSprintTasksInProjectAsync<TEntity, TKey>(
        ProjectId projectId,
        IEnumerable<ISprintTaskId> taskIds,
        CancellationToken token
    )
        where TKey : ISprintTaskId
        where TEntity : class, IProjectOwnedEntity, IEntity<TKey>
    {
        var tasks = await Context
            .Set<TEntity>()
            .Where(t => t.ProjectId == projectId)
            .ToListAsync(token);

        return tasks.Count(t => taskIds.Any(id => id.Value == t.Id.Value));
    }
}