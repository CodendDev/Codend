namespace Codend.Domain.Entities;

public record BugfixProjectTaskProperties(
    string Name,
    string Priority,
    ProjectTaskStatusId StatusId,
    ProjectId ProjectId,
    string? Description = null,
    TimeSpan? EstimatedTime = null,
    DateTime? DueDate = null,
    uint? StoryPoints = null,
    UserId? AssigneeId = null
) : ProjectTaskProperties(
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