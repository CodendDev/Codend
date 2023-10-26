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

    public async Task<bool> TasksInProjectAsync(ProjectId projectId, IEnumerable<ProjectTaskId> taskIds,
        CancellationToken token)
    {
        var dbTasks =
            Context
                .Set<BaseProjectTask>()
                .Where(t => taskIds.Any(id => id == t.Id) && t.ProjectId == projectId);
        var dbTasksCount = await dbTasks.CountAsync(token);
        
        return dbTasksCount == taskIds.Count();
    }
}