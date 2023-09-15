using Codend.Contracts.Abstractions;

namespace Codend.Contracts.Requests.ProjectTasks.Update;

/// <summary>
/// Request used for updating BaseProjectTask
/// </summary>
public interface IUpdateBaseProjectTaskRequest
{
    /// <summary>
    /// Name of the task.
    /// </summary>
    IShouldUpdate<string>? Name { get; }

    /// <summary>
    /// Priority of the task.
    /// </summary>
    IShouldUpdate<string>? Priority { get; }

    /// <summary>
    /// StatusId of the task. Status and task must belong to the same project.
    /// </summary>
    IShouldUpdate<Guid>? StatusId { get; }

    /// <summary>
    /// Description of the task.
    /// </summary>
    IShouldUpdate<string?>? Description { get; }

    /// <summary>
    /// Estimated time of the task completion.
    /// </summary>
    IShouldUpdate<EstimatedTimeRequest>? EstimatedTime { get; }

    /// <summary>
    /// Due date by which task has to be done.
    /// </summary>
    IShouldUpdate<DateTime?>? DueDate { get; }

    /// <summary>
    /// Story points of the task.
    /// </summary>
    IShouldUpdate<uint?>? StoryPoints { get; }

    /// <summary>
    /// UserId of the Assignee.
    /// </summary>
    IShouldUpdate<Guid?>? AssigneeId { get; }

    /// <summary>
    /// StoryId to which task will be assigned.
    /// </summary>
    IShouldUpdate<Guid?>? StoryId { get; }
}