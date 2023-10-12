namespace Codend.Contracts.Responses.Story;

/// <summary>
/// Represents story with limited data response.
/// </summary>
/// <param name="Id">Story Id.</param>
/// <param name="Name">Story name.</param>
/// <param name="EpicId">Story epic id.</param>
/// <param name="StatusId">Story status.</param>
public record BoardStoryResponse(
    Guid Id,
    string Name,
    Guid? EpicId,
    Guid StatusId
);