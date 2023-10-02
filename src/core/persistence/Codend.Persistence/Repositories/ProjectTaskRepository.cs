using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Codend.Persistence.Repositories;

public class ProjectTaskRepository : GenericRepository<ProjectTaskId, Guid, BaseProjectTask>, IProjectTaskRepository
{
    public ProjectTaskRepository(CodendApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public bool ProjectTaskStatusIsValid(ProjectId projectId, ProjectTaskStatusId statusId)
    {
        var valid =
            Context.Set<ProjectTaskStatus>()
                .Any(status => status.ProjectId == projectId && status.Id == statusId);

        return valid;
    }

    /// <inheritdoc/>
    public Task<List<BaseProjectTask>> GetStoryTasksAsync(StoryId storyId, CancellationToken cancellationToken)
    {
        var tasks =
            Context.Set<BaseProjectTask>()
                .Where(task => task.StoryId == storyId)
                .ToListAsync(cancellationToken);

        return tasks;
    }

    /// <inheritdoc/>
    public Task<List<BaseProjectTask>> GetTasksByStatusIdAsync(ProjectTaskStatusId statusId, CancellationToken cancellationToken)
    {
        var tasks =
            Context.Set<BaseProjectTask>()
                .Where(task => task.StatusId == statusId)
                .ToListAsync(cancellationToken);

        return tasks;
    }
}