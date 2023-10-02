using Codend.Domain.Entities.ProjectTask.Abstractions;

namespace Codend.Domain.Entities.ProjectTask.Bugfix;

public record BugfixProjectTaskCreateProperties
(
    ProjectId ProjectId,
    string Name,
    string Priority,
    string? Description,
    TimeSpan? EstimatedTime,
    DateTime? DueDate,
    uint? StoryPoints,
    UserId? AssigneeId,
    StoryId? StoryId
) : IProjectTaskCreateProperties
{
    public ProjectTaskStatusId? StatusId { get; set; }
}