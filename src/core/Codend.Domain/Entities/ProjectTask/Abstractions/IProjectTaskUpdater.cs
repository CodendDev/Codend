using Codend.Shared;
using FluentResults;

namespace Codend.Domain.Entities.ProjectTask.Abstractions;

public record AbstractProjectTaskUpdateProperties
(
    IShouldUpdate<string> Name,
    IShouldUpdate<string> Priority,
    IShouldUpdate<ProjectTaskStatusId> StatusId,
    IShouldUpdate<string?> Description,
    IShouldUpdate<TimeSpan?> EstimatedTime,
    IShouldUpdate<DateTime?> DueDate,
    IShouldUpdate<uint?> StoryPoints,
    IShouldUpdate<UserId?> AssigneeId
);

public interface IProjectTaskUpdater<TProjectTask, in TUpdateProps>
    where TProjectTask : AbstractProjectTask
    where TUpdateProps : AbstractProjectTaskUpdateProperties
{
    Result<TProjectTask> Update(TUpdateProps properties);
}