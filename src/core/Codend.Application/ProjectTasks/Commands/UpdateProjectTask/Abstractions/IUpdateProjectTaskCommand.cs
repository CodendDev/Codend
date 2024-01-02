using Codend.Contracts.Requests;
using Codend.Domain.Entities;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;

/// <summary>
/// Interface used for updating <see cref="BaseProjectTask"/> properties.
/// </summary>
public interface IUpdateProjectTaskCommand
{
    /// <summary>
    /// Identifier of the project containing the task that is being updated.
    /// </summary>
    ProjectId ProjectId { get; }

    /// <summary>
    /// Id of <see cref="BaseProjectTask"/> which will be updated.
    /// </summary>
    ProjectTaskId TaskId { get; }

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
    ProjectTaskStatusId? StatusId { get; }

    /// <summary>
    /// Description of the task.
    /// </summary>
    ShouldUpdateBinder<string?> Description { get; }

    /// <summary>
    /// Estimated time of the task completion.
    /// </summary>
    ShouldUpdateBinder<TimeSpan?> EstimatedTime { get; }

    /// <summary>
    /// Due date by which task has to be done.
    /// </summary>
    ShouldUpdateBinder<DateTime?> DueDate { get; }

    /// <summary>
    /// Story points of the task.
    /// </summary>
    ShouldUpdateBinder<uint?> StoryPoints { get; }

    /// <summary>
    /// UserId of the Assignee.
    /// </summary>
    ShouldUpdateBinder<UserId?> AssigneeId { get; }

    /// <summary>
    /// StoryId to which task will be assigned.
    /// </summary>
    ShouldUpdateBinder<StoryId?> StoryId { get; }
}