namespace Codend.Contracts.Requests.ProjectTasks.Create;

/// <summary>
/// Request used for creating BaseProjectTask
/// </summary>
public interface ICreateBaseProjectTaskRequest
{
    /// <summary>
    /// Name of the task.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Priority of the task.
    /// </summary>
    string Priority { get; }

    /// <summary>
    /// StatusId of the task. Status and task must belong to the same project.
    /// </summary>
    Guid StatusId { get; }

    /// <summary>
    /// Description of the task.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Estimated time of the task completion.
    /// </summary>
    EstimatedTimeRequest? EstimatedTime { get; }

    /// <summary>
    /// Due date by which task has to be done.
    /// </summary>
    DateTime? DueDate { get; }

    /// <summary>
    /// Story points of the task.
    /// </summary>
    uint? StoryPoints { get; }

    /// <summary>
    /// UserId of the Assignee.
    /// </summary>
    Guid? AssigneeId { get; }

    /// <summary>
    /// StoryId to which task will be assigned.
    /// </summary>
    Guid? StoryId { get; }
}