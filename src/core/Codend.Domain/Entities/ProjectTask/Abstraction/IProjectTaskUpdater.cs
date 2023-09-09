using Codend.Shared.ShouldUpdate;
using FluentResults;

namespace Codend.Domain.Entities;

public abstract record UpdateProjectTaskProperties
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
    where TProjectTask : ProjectTask
    where TUpdateProps : UpdateProjectTaskProperties
{
    Result<TProjectTask> Update(TUpdateProps properties);
}