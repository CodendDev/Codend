using Codend.Contracts.Requests.Sprint;

namespace Codend.Contracts.Responses.Sprint;

/// <summary>
/// Represents sprints response.
/// </summary>
/// <param name="ActiveSprint">Id of sprint which is currently active.</param>
/// <param name="Sprints">List of all project sprints.</param>
public record SprintsResponse
(
    Guid? ActiveSprint,
    IEnumerable<SprintTaskRequest> Sprints
);