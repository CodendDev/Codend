namespace Codend.Contracts.Requests.Epic;

/// <summary>
/// Represents create epic request.
/// </summary>
/// <param name="Name">Name of the epic.</param>
/// <param name="Description">Description of the epic.</param>
public record CreateEpicRequest
(
    string Name,
    string Description
);