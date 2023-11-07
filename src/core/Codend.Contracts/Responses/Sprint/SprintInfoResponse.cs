namespace Codend.Contracts.Responses.Sprint;

/// <summary>
/// Represents sprint information response.
/// </summary>
/// <param name="Id">Sprint id.</param>
/// <param name="Name">Sprint name.</param>
/// <param name="StartDate">Sprint start date.</param>
/// <param name="EndDate">Sprint end date.</param>
/// <param name="Goal">Sprint goal.</param>
public record SprintInfoResponse
(
    Guid Id,
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    string? Goal
);