namespace Codend.Contracts.Requests.ProjectTaskStatuses;

/// <summary>
/// Request with properties used for updating ProjectTaskStatus.
/// </summary>
/// <param name="Name">Task status name.</param>
public sealed record UpdateProjectTaskStatusRequest
(
    string Name
);