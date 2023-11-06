namespace Codend.Contracts.Responses.Sprint;

/// <summary>
/// Represents sprints response.
/// </summary>
/// <param name="ActiveSprints">Ids of sprints that are currently active.</param>
/// <param name="Sprints">List of all project sprints.</param>
public record SprintsResponse
(
    IEnumerable<Guid> ActiveSprints,
    IEnumerable<SprintResponse> Sprints
);