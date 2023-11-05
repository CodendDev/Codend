using Codend.Contracts.Responses.Epic;
using Codend.Contracts.Responses.ProjectTask;
using Codend.Contracts.Responses.Story;

namespace Codend.Contracts.Responses.Board;

/// <summary>
/// Represents combined project tasks, stories and epics response.
/// </summary>
/// <param name="Tasks">Project board tasks.</param>

public record BoardResponse(
    IEnumerable<BoardTaskResponse> Tasks
);