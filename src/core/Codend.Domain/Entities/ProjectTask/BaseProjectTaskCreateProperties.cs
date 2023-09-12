using Codend.Domain.Entities.ProjectTask.Abstractions;

namespace Codend.Domain.Entities;

public record BaseProjectTaskCreateProperties
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
) : IProjectTaskCreateProperties;