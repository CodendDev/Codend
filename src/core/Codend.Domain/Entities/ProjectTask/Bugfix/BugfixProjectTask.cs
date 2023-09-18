using Codend.Domain.Entities.ProjectTask.Abstractions;
using FluentResults;

namespace Codend.Domain.Entities.ProjectTask.Bugfix;

public class BugfixProjectTask :
    BaseProjectTask,
    IProjectTaskCreator<BugfixProjectTask, BugfixProjectTaskCreateProperties>
{
    private BugfixProjectTask()
    {
    }

    public static Result<BugfixProjectTask> Create(BugfixProjectTaskCreateProperties properties, UserId ownerId)
    {
        var task = new BugfixProjectTask();
        var result = task.PopulateBaseProperties(properties, ownerId);

        if (result.IsFailed)
        {
            return result.ToResult();
        }

        return Result.Ok(task);
    }
}