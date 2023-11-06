namespace Codend.Contracts.Requests.ProjectTaskStatuses;

/// <summary>
/// Represents MoveProjectTaskStatusCommand request.
/// </summary>
/// <param name="Prev">Value after which status will be positioned.</param>
/// <param name="Next">Value before which status will be positioned.</param>
public record MoveProjectTaskStatusRequest(string? Prev, string? Next);