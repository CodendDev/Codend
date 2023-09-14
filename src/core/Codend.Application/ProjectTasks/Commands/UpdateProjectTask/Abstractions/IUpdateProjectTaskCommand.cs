using Codend.Contracts.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;

/// <summary>
/// Interface used for updating <see cref="BaseProjectTask"/> properties.
/// </summary>
public interface IUpdateProjectTaskCommand
{
    /// <summary>
    /// Id of <see cref="BaseProjectTask"/> which will be updated.
    /// </summary>
    ProjectTaskId TaskId { get; }

    /// <summary>
    /// Name of the task.
    /// </summary>
    IShouldUpdate<string> Name { get; }

    /// <summary>
    /// Priority of the task.
    /// </summary>
    IShouldUpdate<string> Priority { get; }

    /// <summary>
    /// StatusId of the task. Status and task must belong to the same project.
    /// </summary>
    IShouldUpdate<ProjectTaskStatusId> StatusId { get; }

    /// <summary>
    /// Description of the task.
    /// </summary>
    IShouldUpdate<string?> Description { get; }

    /// <summary>
    /// Estimated time of the task completion.
    /// </summary>
    IShouldUpdate<TimeSpan?> EstimatedTime { get; }

    /// <summary>
    /// Due date by which task has to be done.
    /// </summary>
    IShouldUpdate<DateTime?> DueDate { get; }

    /// <summary>
    /// Story points of the task.
    /// </summary>
    IShouldUpdate<uint?> StoryPoints { get; }

    /// <summary>
    /// UserId of the Assignee.
    /// </summary>
    IShouldUpdate<UserId?> AssigneeId { get; }

    /// <summary>
    /// StoryId to which task will be assigned.
    /// </summary>
    IShouldUpdate<StoryId?> StoryId { get; }
}