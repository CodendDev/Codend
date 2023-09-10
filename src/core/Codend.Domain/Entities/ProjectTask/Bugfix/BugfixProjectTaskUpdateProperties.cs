using Codend.Domain.Entities.ProjectTask.Abstractions;
using Codend.Shared;

namespace Codend.Domain.Entities.ProjectTask.Bugfix;

public record BugfixProjectTaskUpdateProperties
(
    IShouldUpdate<string> Name,
    IShouldUpdate<string> Priority,
    IShouldUpdate<ProjectTaskStatusId> StatusId,
    IShouldUpdate<string?> Description,
    IShouldUpdate<TimeSpan?> EstimatedTime,
    IShouldUpdate<DateTime?> DueDate,
    IShouldUpdate<uint?> StoryPoints,
    IShouldUpdate<UserId?> AssigneeId
) : AbstractProjectTaskUpdateProperties(
    Name,
    Priority,
    StatusId,
    Description,
    EstimatedTime,
    DueDate,
    StoryPoints,
    AssigneeId
);