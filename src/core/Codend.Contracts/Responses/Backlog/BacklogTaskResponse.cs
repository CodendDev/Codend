namespace Codend.Contracts.Responses.Backlog;

/// <summary>
/// Represents compact response of all project task/elements.
/// </summary>
/// <param name="Id">Id of the task.</param>
/// <param name="Name">Name of the task.</param>
/// <param name="TaskType">Type of the task. Either Base, Bugfix, Epic or Story.</param>
/// <param name="StatusName">Task status.</param>
/// <param name="AssigneeAvatar">Avatar of assigned user, if there is one.</param>
/// <param name="CreatedOn">Creation datetime.</param>
public record BacklogTaskResponse
(
    Guid Id,
    string Name,
    string TaskType,
    string StatusName,
    string? AssigneeAvatar,
    DateTime CreatedOn
);