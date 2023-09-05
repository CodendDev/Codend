namespace Codend.Contracts.ProjectTasks;

public record CreateBugfixRequest
(
    string Name,
    string Priority,
    Guid Status,
    Guid ProjectId,
    string? Description = null,
    TimeSpan? EstimatedTime = null,
    DateTime? DueDate = null,
    uint? StoryPoints = null,
    Guid? AssigneeId = null
);