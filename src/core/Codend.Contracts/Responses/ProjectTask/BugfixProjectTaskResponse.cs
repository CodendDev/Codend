namespace Codend.Contracts.Responses.ProjectTask;

/// <summary>
/// Represents BugfixProjectTask response.
/// </summary>
/// <param name="Id"><inheritdoc/></param>
/// <param name="TaskType"><inheritdoc/></param>
/// <param name="Name"><inheritdoc/></param>
/// <param name="Priority"><inheritdoc/></param>
/// <param name="Description"><inheritdoc/></param>
/// <param name="DueDate"><inheritdoc/></param>
/// <param name="StoryPoints"><inheritdoc/></param>
/// <param name="AssigneeId"><inheritdoc/></param>
/// <param name="EstimatedTime"><inheritdoc/></param>
/// <param name="StoryId"><inheritdoc/></param>
public record BugfixProjectTaskResponse
(
    Guid Id,
    string TaskType,
    string Name,
    string Priority,
    string? Description,
    DateTime? DueDate,
    uint? StoryPoints,
    Guid? AssigneeId,
    EstimatedTimeResponse? EstimatedTime,
    Guid? StoryId
) : BaseProjectTaskResponse
(
    Id,
    TaskType,
    Name,
    Priority,
    Description,
    DueDate,
    StoryPoints,
    AssigneeId,
    EstimatedTime,
    StoryId
);