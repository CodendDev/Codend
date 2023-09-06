namespace Codend.Contracts.ProjectTasks;

public record CreateBugfixRequest
(
    string Name,
    string Priority,
    Guid StatusId,
    Guid ProjectId,
    string? Description = null,
    EstimatedTimeRequest? EstimatedTime = null,
    DateTime? DueDate = null,
    uint? StoryPoints = null,
    Guid? AssigneeId = null
);