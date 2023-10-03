namespace Codend.Contracts.Responses.Epic;

/// <summary>
/// Represents default epic response.
/// </summary>
/// <param name="Id">Epic Id.</param>
/// <param name="Name">Epic name.</param>
/// <param name="Description">Epic desription.</param>
/// <param name="StatusId">Epic status.</param>
public record EpicResponse(
    Guid Id,
    string Name,
    string Description,
    Guid StatusId
);