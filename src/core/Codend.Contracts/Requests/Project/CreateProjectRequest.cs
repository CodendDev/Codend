namespace Codend.Contracts.Requests.Project;

/// <summary>
/// Represents the create project request.
/// </summary>
/// <param name="Name">Name of the project.</param>
/// <param name="Description">Project description.</param>
public sealed record CreateProjectRequest(
    string Name,
    string? Description);