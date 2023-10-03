namespace Codend.Contracts.Responses.Story;

/// <summary>
/// Represents default story response.
/// </summary>
/// <param name="Id">Story Id.</param>
/// <param name="Name">Story name.</param>
/// <param name="Description">Story desription.</param>
/// <param name="EpicId">Story epic id.</param>
/// <param name="StatusId">Story status.</param>
public record StoryResponse(
    Guid Id,
    string Name,
    string Description,
    Guid? EpicId,
    Guid StatusId
);