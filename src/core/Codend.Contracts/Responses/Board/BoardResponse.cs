using Codend.Contracts.Responses.Epic;
using Codend.Contracts.Responses.ProjectTask;
using Codend.Contracts.Responses.Story;

namespace Codend.Contracts.Responses.Board;

/// <summary>
/// Represesnts combined project tasks, stories and epics response.
/// </summary>
/// <param name="Tasks">Project tasks.</param>
/// <param name="Stories">Project stories.</param>
/// <param name="Epics">Project epics.</param>
public record BoardResponse(
    IEnumerable<BoardProjectTaskResponse> Tasks,
    IEnumerable<BoardStoryResponse> Stories,
    IEnumerable<BoardEpicResponse> Epics
);