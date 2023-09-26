using Codend.Domain.Entities;
using Codend.Domain.Repositories;

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
    public IEnumerable<BaseProjectTask> GetStoryTasks(StoryId storyId)
    {
        var tasks =
            Context.Set<BaseProjectTask>()
                .Where(task => task.StoryId == storyId);

        return tasks;
    }

    public IEnumerable<BaseProjectTask> GetTasksByTaskStatusId(ProjectTaskStatusId statusId)
    {
        var tasks =
            Context.Set<BaseProjectTask>()
                .Where(task => task.StatusId == statusId);

        return tasks;
    }
}