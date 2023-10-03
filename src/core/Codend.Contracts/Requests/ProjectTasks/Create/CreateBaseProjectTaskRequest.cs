namespace Codend.Contracts.Requests.ProjectTasks.Create;

/// <inheritdoc />
public record CreateBaseProjectTaskRequest
(
    string Name,
    string Priority,
    Guid? StatusId,
    string? Description,
    EstimatedTimeRequest? EstimatedTime,
    DateTime? DueDate,
    uint? StoryPoints,
    Guid? AssigneeId,
    Guid? StoryId
) : ICreateBaseProjectTaskRequest;