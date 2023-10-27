namespace Codend.Contracts.Requests.Sprint;

/// <summary>
/// Represents create sprint request.
/// </summary>
/// <param name="Name">Name of the sprint.</param>
/// <param name="StartDate">Start date of the sprint.</param>
/// <param name="EndDate">End date of the sprint.</param>
/// <param name="Goal">Goal of the sprint.</param>
public record CreateSprintRequest
(
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    string? Goal
);