namespace Codend.Contracts.Requests.Sprint;

/// <summary>
/// Represents MoveSprintTask request.
/// </summary>
/// <param name="Prev">Lexorank position value after which task will be inserted.</param>
/// <param name="Next">Lexorank position value before which task will be inserted.</param>
/// <param name="TaskRequest">Id and the type of task that will be moved.</param>
/// <param name="StatusId">New status for the task.</param>
public record MoveSprintTaskRequest(string? Prev, string? Next, SprintTaskRequest TaskRequest, Guid? StatusId);