using FluentResults;

namespace Codend.Domain.Entities;

public class BugfixProjectTask :
    ProjectTask,
    IPersistentProjectTask<BugfixProjectTask, BugfixProjectTaskProperties, BugfixUpdateProjectTaskProperties>
{
    private BugfixProjectTask(ProjectTaskId id) : base(id)
    {
    }

    public static Result<BugfixProjectTask> Create(BugfixProjectTaskProperties properties)
    {
        var task = new BugfixProjectTask(new ProjectTaskId(Guid.NewGuid()));
        var result = task.Create(properties as ProjectTaskProperties);

        if (result.IsFailed)
        {
            return result.ToResult();
        }

        return Result.Ok(task);
    }

    public Result<BugfixProjectTask> Update(BugfixUpdateProjectTaskProperties properties)
    {
        var result = UpdateBase(properties);
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        return Result.Ok();
    }
}