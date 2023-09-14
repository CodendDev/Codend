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
    public bool ProjectTaskIsValid(ProjectId projectId, ProjectTaskStatusId statusId)
    {
        var valid =
            Context.Set<ProjectTaskStatus>()
                .Any(status => status.ProjectId == projectId && status.Id == statusId);

        return valid;
    }

    /// <inheritdoc/>
    public IEnumerable<BaseProjectTask> GetStoryTasks(StoryId storyId)
    {
        var tasks =
            Context.Set<BaseProjectTask>()
                .Where(task => task.StoryId == storyId);

        return tasks;
    }
}