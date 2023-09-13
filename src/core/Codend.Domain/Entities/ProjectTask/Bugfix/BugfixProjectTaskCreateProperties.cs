using Codend.Domain.Entities.ProjectTask.Abstractions;

namespace Codend.Domain.Entities.ProjectTask.Bugfix;

public record BugfixProjectTaskCreateProperties
(
    string Name,
    string Priority,
    ProjectTaskStatusId StatusId,
    ProjectId ProjectId,
    string? Description,
    TimeSpan? EstimatedTime,
    DateTime? DueDate,
    uint? StoryPoints,
    UserId? AssigneeId,
    StoryId? StoryId
) : IProjectTaskCreateProperties;