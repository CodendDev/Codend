namespace Codend.Contracts.Responses.ProjectTask;

public record BugfixProjectTaskResponse
(
    string TaskType,
    string Name,
    string Priority,
    string? Description,
    DateTime? DueDate,
    uint? StoryPoints,
    Guid? AssigneeId,
    EstimatedTimeResponse? EstimatedTime
) : BaseProjectTaskResponse
(
    TaskType,
    Name,
    Priority,
    Description,
    DueDate,
    StoryPoints,
    AssigneeId,
    EstimatedTime
);