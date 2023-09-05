using Codend.Domain.Core.Enums;
using FluentResults;

namespace Codend.Domain.Entities;

public record BugFixProjectTaskProperties(
        string Name,
        UserId OwnerId,
        ProjectTaskPriority Priority,
        ProjectTaskStatusId StatusId,
        ProjectId ProjectId,
        string? Description = null,
        TimeSpan? EstimatedTime = null,
        DateTime? DueDate = null,
        uint? StoryPoints = null,
        UserId? AssigneeId = null)
    : ProjectTaskProperties(Name,
        OwnerId,
        Priority,
        StatusId,
        ProjectId,
        Description,
        EstimatedTime,
        DueDate,
        StoryPoints,
        AssigneeId);

public class BugFixProjectTask : ProjectTask, IProjectTaskCreator<BugFixProjectTask, BugFixProjectTaskProperties>
{
    private BugFixProjectTask(ProjectTaskId id) : base(id)
    {
    }

    public static Result<BugFixProjectTask> Create(BugFixProjectTaskProperties properties)
    {
        var task = new BugFixProjectTask(new ProjectTaskId(Guid.NewGuid()));
        var result = task.Create(properties as ProjectTaskProperties);

        if (result.IsFailed)
        {
            return result.ToResult();
        }

        return Result.Ok(task);
    }
}