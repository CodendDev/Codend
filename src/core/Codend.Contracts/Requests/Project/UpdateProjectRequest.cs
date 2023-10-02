namespace Codend.Contracts.Requests.Project;

/// <summary>
/// Represents the update project request.
/// </summary>
/// <param name="Name">Name of the project.</param>
/// <param name="Description">Project description.</param>
/// <param name="DefaultStatusId">Default project status Id.</param>
public sealed record UpdateProjectRequest(
    string Name,
    ShouldUpdateBinder<string?> Description,
    Guid DefaultStatusId);