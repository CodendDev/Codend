namespace Codend.Contracts.Responses.ProjectTaskStatus;

/// <summary>
/// Represents default project task status response.
/// </summary>
/// <param name="Id">Id of the status.</param>
/// <param name="Name">Name of the status.</param>
public sealed record ProjectTaskStatusResponse
(
    Guid Id,
    string Name
);