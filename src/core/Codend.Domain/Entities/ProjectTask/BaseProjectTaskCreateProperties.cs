using Codend.Domain.Entities.ProjectTask.Abstractions;

namespace Codend.Domain.Entities;

public record BaseProjectTaskCreateProperties
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