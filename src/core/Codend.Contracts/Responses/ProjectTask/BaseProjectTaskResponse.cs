namespace Codend.Contracts.Responses.ProjectTask;

/// <summary>
/// Represents BaseProjectTask response.
/// </summary>
/// <param name="TaskType">Type of the task.</param>
/// <param name="Name">Name of the task.</param>
/// <param name="Priority">Priority of the task.</param>
/// <param name="Description">Description of the task.</param>
/// <param name="DueDate">Due date of the task.</param>
/// <param name="StoryPoints">Story points of the task.</param>
/// <param name="AssigneeId">User assigned to the task.</param>
/// <param name="EstimatedTime">Estimated time of the task.</param>
public record BaseProjectTaskResponse
(
    string TaskType,
    string Name,
    string Priority,
    string? Description,
    DateTime? DueDate,
    uint? StoryPoints,
    Guid? AssigneeId,
    EstimatedTimeResponse? EstimatedTime
)
{
    /// <summary>
    /// Status of the task.
    /// </summary>
    public string Status { get; set; }
};