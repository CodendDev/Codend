namespace Codend.Contracts.Requests.Story;

/// <summary>
/// Represents update story request.
/// </summary>
/// <param name="Name">Name of the story.</param>
/// <param name="Description">Description of the story.</param>
/// <param name="EpicId">EpicId of the story.</param>
public record UpdateStoryRequest(
    string Name,
    string Description,
    ShouldUpdateBinder<Guid?>? EpicId
);