namespace Codend.Contracts.Requests.Sprint;

/// <summary>
/// Sprint request with list of tasks ids. Used for managing tasks in sprints.
/// </summary>
/// <param name="Tasks">List of tasks ids.</param>
public record SprintTasksRequest(List<SprintTaskRequest> Tasks);