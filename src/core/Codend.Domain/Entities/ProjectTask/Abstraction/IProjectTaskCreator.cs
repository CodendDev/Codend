using Codend.Domain.Core.Enums;
using FluentResults;

namespace Codend.Domain.Entities;

/// <summary>
/// Base ProjectTask properties.
/// </summary>
public abstract record ProjectTaskProperties(
    string Name,
    string Priority,
    ProjectTaskStatusId StatusId,
    ProjectId ProjectId,
    string? Description = null,
    TimeSpan? EstimatedTime = null,
    DateTime? DueDate = null,
    uint? StoryPoints = null,
    UserId? AssigneeId = null
)
{
    public UserId? OwnerId { get; set; }
};

/// <summary>
/// Interface for <see cref="IProjectTaskCreator{TProjectTask,TProps}"/>.
/// ProjectTask which implements this interface can be created using <see cref="IProjectTaskCreator{TProjectTask,TProps}.Create"/> method.
/// </summary>
/// <typeparam name="TProjectTask">ProjectTask class.</typeparam>
/// <typeparam name="TProps">Props class which will be used for creation.</typeparam>
public interface IProjectTaskCreator<TProjectTask, in TProps>
    where TProjectTask : ProjectTask
    where TProps : ProjectTaskProperties
{
    static abstract Result<TProjectTask> Create(TProps props);
}