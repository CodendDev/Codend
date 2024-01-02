namespace Codend.Contracts.Responses.Backlog;

/// <summary>
/// Represents project backlog response.
/// </summary>
/// <param name="Tasks">Tasks list containing all baseTasks/bugfixes/epics/stories in compact form.</param>
public record BacklogResponse(IEnumerable<BacklogTaskResponse> Tasks);