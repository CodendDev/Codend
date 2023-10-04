namespace Codend.Contracts.Responses.Epic;

/// <summary>
/// Represents epic with limited data response.
/// </summary>
/// <param name="Id">Epic Id.</param>
/// <param name="Name">Epic name.</param>
/// <param name="StatusId">Epic status.</param>
public record BoardEpicResponse(
    Guid Id,
    string Name,
    Guid StatusId
);