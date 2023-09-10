using Codend.Domain.Entities.ProjectTask.Abstractions;

namespace Codend.Domain.Entities.ProjectTask.Bugfix;

public record BugfixProjectTaskCreateProperties
(
    string Name,
    string Priority,
    ProjectTaskStatusId StatusId,
    ProjectId ProjectId,
    string? Description = null,
    TimeSpan? EstimatedTime = null,
    DateTime? DueDate = null,
    uint? StoryPoints = null,
    UserId? AssigneeId = null
) : AbstractProjectTaskCreateProperties(
    Name,
    Priority,
    StatusId,
    ProjectId,
    Description,
    EstimatedTime,
    DueDate,
    StoryPoints,
    AssigneeId
);