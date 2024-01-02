namespace Codend.Contracts.Requests.Project;

/// <summary>
/// Represents the update project IsFavourite flag request.
/// </summary>
/// <param name="IsFavourite">New, different than current isFavourite value.</param>
public sealed record UpdateProjectIsFavouriteFlagRequest(bool IsFavourite);