namespace Codend.Contracts.Responses.Project;

/// <summary>
/// Represents default project response.
/// </summary>
/// <param name="Id">Id of the project.</param>
/// <param name="Name">Name of the project.</param>
/// <param name="Description">Description of the project.</param>
/// <param name="OwnerId">Id of the project owner.</param>
/// <param name="IsFavourite">Is the project user's favourite.</param>
public sealed record ProjectResponse(
    Guid Id,
    string Name,
    string? Description,
    Guid OwnerId,
    bool IsFavourite);