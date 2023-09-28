namespace Codend.Contracts.Responses.ProjectTaskStatus;

/// <summary>
/// Represents default project task status response.
/// </summary>
/// <param name="Name">Name of the status.</param>
public sealed record ProjectTaskStatusResponse
(
    string Name
);