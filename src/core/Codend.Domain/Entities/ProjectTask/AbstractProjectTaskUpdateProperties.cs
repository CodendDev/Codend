using Codend.Shared;

namespace Codend.Domain.Entities.ProjectTask;

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