using Codend.Shared;

namespace Codend.Domain.Entities;

public record BugfixUpdateProjectTaskProperties(
    IShouldUpdate<string> Name,
    IShouldUpdate<string> Priority,
    IShouldUpdate<ProjectTaskStatusId> StatusId,
    IShouldUpdate<string?> Description,
    IShouldUpdate<TimeSpan?> EstimatedTime,
    IShouldUpdate<DateTime?> DueDate,
    IShouldUpdate<uint?> StoryPoints,
    IShouldUpdate<UserId?> AssigneeId
) : UpdateAbstractProjectTaskProperties(
    Name,
    Priority,
    StatusId,
    Description,
    EstimatedTime,
    DueDate,
    StoryPoints,
    AssigneeId
);