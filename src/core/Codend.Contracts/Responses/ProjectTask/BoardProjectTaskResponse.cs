namespace Codend.Contracts.Responses.ProjectTask;

/// <summary>
/// Represents BoardProjectTask response, with minimal data required to display.
/// </summary>
/// <param name="Id">Id of the task.</param>
/// <param name="TaskType">Type of the task.</param>
/// <param name="Name">Name of the task.</param>
/// <param name="Priority">Priority of the task.</param>
/// <param name="StatusId">Status of the task.</param>
/// <param name="StoryId">Id of the story tasks is assigned to.</param>
/// <param name="AssigneeId">User assigned to the task.</param>
public record BoardProjectTaskResponse
(
    Guid Id,
    string TaskType,
    string Name,
    string Priority,
    Guid StatusId,
    Guid? StoryId,
    Guid? AssigneeId
);