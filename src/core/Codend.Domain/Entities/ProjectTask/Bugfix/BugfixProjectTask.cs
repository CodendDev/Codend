using Codend.Domain.Entities.ProjectTask.Abstractions;
using FluentResults;

namespace Codend.Domain.Entities.ProjectTask.Bugfix;

public class BugfixProjectTask :
    AbstractProjectTask,
    IProjectTaskCreator<BugfixProjectTask, BugfixProjectTaskCreateProperties>
{
    private BugfixProjectTask(ProjectTaskId id) : base(id)
    {
    }

    public static Result<BugfixProjectTask> Create(BugfixProjectTaskCreateProperties properties, UserId ownerId)
    {
        var task = new BugfixProjectTask(new ProjectTaskId(Guid.NewGuid()));
        var result = task.Create(properties as IProjectTaskCreateProperties, ownerId);

        if (result.IsFailed)
        {
            return result.ToResult();
        }

        return Result.Ok(task);
    }
}