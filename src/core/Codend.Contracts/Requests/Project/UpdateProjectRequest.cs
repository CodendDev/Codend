namespace Codend.Contracts.Requests.Project;

/// <summary>
/// Represents the update project request.
/// </summary>
/// <param name="Name">Name of the project.</param>
/// <param name="Description">Project description.</param>
public sealed record UpdateProjectRequest(
    string Name,
    string? Description);