using Codend.Domain.Core.Primitives;
using FluentResults;

namespace Codend.Domain.Entities;

public class SprintProjectTask : Entity<SprintProjectTaskId>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private SprintProjectTask() : base(new SprintProjectTaskId(Guid.NewGuid()))
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public SprintId SprintId { get; private set; }
    public ProjectTaskId TaskId { get; private set; }

    public static Result<SprintProjectTask> Create(SprintId sprintId, ProjectTaskId taskId) =>
        Result.Ok(new SprintProjectTask()
        {
            SprintId = sprintId,
            TaskId = taskId,
        });
}