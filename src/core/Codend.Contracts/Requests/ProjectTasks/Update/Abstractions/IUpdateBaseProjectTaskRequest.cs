namespace Codend.Contracts.Requests.ProjectTasks.Update.Abstractions;

/// <summary>
/// Request used for updating BaseProjectTask
/// </summary>
public interface IUpdateBaseProjectTaskRequest
{
    /// <summary>
    /// Name of the task.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Priority of the task.
    /// </summary>
    string? Priority { get; }

    /// <summary>
    /// StatusId of the task. Status and task must belong to the same project.
    /// </summary>
    Guid? StatusId { get; }

    /// <summary>
    /// Description of the task.
    /// </summary>
    ShouldUpdateBinder<string?>? Description { get; }

    /// <summary>
    /// Estimated time of the task completion.
    /// </summary>
    ShouldUpdateBinder<EstimatedTimeRequest?>? EstimatedTime { get; }

    /// <summary>
    /// Due date by which task has to be done.
    /// </summary>
    ShouldUpdateBinder<DateTime?>? DueDate { get; }

    /// <summary>
    /// Story points of the task.
    /// </summary>
    ShouldUpdateBinder<uint?>? StoryPoints { get; }

    /// <summary>
    /// UserId of the Assignee.
    /// </summary>
    ShouldUpdateBinder<Guid?>? AssigneeId { get; }

    /// <summary>
    /// StoryId to which task will be assigned.
    /// </summary>
    ShouldUpdateBinder<Guid?>? StoryId { get; }
}