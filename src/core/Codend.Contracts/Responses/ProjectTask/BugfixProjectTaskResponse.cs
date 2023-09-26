namespace Codend.Contracts.Responses.ProjectTask;

/// <summary>
/// Represents BugfixProjectTask response.
/// </summary>
/// <param name="TaskType"><inheritdoc/></param>
/// <param name="Name"><inheritdoc/></param>
/// <param name="Priority"><inheritdoc/></param>
/// <param name="Description"><inheritdoc/></param>
/// <param name="DueDate"><inheritdoc/></param>
/// <param name="StoryPoints"><inheritdoc/></param>
/// <param name="AssigneeId"><inheritdoc/></param>
/// <param name="EstimatedTime"><inheritdoc/></param>
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