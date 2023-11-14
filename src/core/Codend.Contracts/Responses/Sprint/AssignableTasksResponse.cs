namespace Codend.Contracts.Responses.Sprint;

/// <summary>
/// Represents assignable to sprint tasks response.
/// </summary>
/// <param name="Tasks">Tasks list containing all assignable tasks.</param>
public record AssignableTasksResponse(IEnumerable<AssignableTaskResponse> Tasks);

/// <summary>
/// Represents assignable to sprint task.
/// </summary>
/// <param name="Id">Id of the task.</param>
/// <param name="Name">Name of the task.</param>
/// <param name="TaskType">Type of the task. Either Base, Bugfix, Epic or Story.</param>
/// <param name="StatusName">Task status.</param>
public record AssignableTaskResponse
(
    Guid Id,
    string Name,
    string TaskType,
    string StatusName
);