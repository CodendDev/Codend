namespace Codend.Contracts.Requests.ProjectTaskStatuses;

/// <summary>
/// Request with properties used for creating ProjectTaskStatus.
/// </summary>
/// <param name="Name">Task status name.</param>
public sealed record CreateProjectTaskStatusRequest
(
    string Name
);