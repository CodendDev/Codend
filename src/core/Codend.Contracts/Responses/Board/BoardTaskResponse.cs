namespace Codend.Contracts.Responses.Board;

/// <summary>
/// Represents board task response.
/// </summary>
/// <param name="Id">Id of the board task.</param>
/// <param name="TaskType">Type of the board task (Base, Bugfix, Story, Epic).</param>
/// <param name="Name">Name of the board task.</param>
/// <param name="StatusId">StatusId of the board task.</param>
/// <param name="RelatedTaskId">RelatedTaskId of the board task.</param>
/// <param name="Priority">Priority of the board task.</param>
/// <param name="AssigneeAvatar">AssigneeAvatar of the board task.</param>
/// <param name="Position">Position of the board task.</param>
public record BoardTaskResponse
(
    Guid Id,
    string TaskType,
    string Name,
    Guid StatusId,
    Guid? RelatedTaskId,
    string? Priority,
    string? AssigneeAvatar,
    string? Position
);