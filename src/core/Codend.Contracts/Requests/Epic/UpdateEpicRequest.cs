namespace Codend.Contracts.Requests.Epic;

/// <summary>
/// Represents update epic request.
/// </summary>
/// <param name="Name">Name of the epic.</param>
/// <param name="Description">Description of the epic.</param>
/// <param name="StatusId">Epic status.</param>
public record UpdateEpicRequest(
    string? Name,
    string? Description,
    Guid? StatusId
);