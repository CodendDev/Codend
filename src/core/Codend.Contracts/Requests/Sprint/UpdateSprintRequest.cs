namespace Codend.Contracts.Requests.Sprint;

/// <summary>
/// Represents sprint update request.
/// </summary>
/// <param name="Name">New name of the sprint.</param>
/// <param name="StartDate">New start date of the sprint.</param>
/// <param name="EndDate">New end date of the sprint.</param>
/// <param name="Goal">New goal of the sprint.</param>
public record UpdateSprintRequest(
    string? Name,
    DateTime? StartDate,
    DateTime? EndDate,
    ShouldUpdateBinder<string?>? Goal
);