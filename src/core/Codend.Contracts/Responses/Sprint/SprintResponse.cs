using Codend.Contracts.Responses.Board;

namespace Codend.Contracts.Responses.Sprint;

/// <summary>
/// Represents sprint response.
/// </summary>
/// <param name="Id">Sprint id.</param>
/// <param name="Name">Sprint name.</param>
/// <param name="StartDate">Sprint start date.</param>
/// <param name="EndDate">Sprint end date.</param>
/// <param name="Goal">Sprint goal.</param>
/// <param name="SprintTasks">Sprint tasks.</param>
public record SprintResponse
(
    Guid Id,
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    string? Goal,
    BoardResponse SprintTasks
);