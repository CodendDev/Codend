using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Enums;
using Codend.Domain.Core.Events;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;
using FluentResults;
using InvalidPriorityName = Codend.Domain.Core.Errors.DomainErrors.ProjectTaskPriority.InvalidPriorityName;

namespace Codend.Domain.Entities;

public abstract class AbstractProjectTask :
    Aggregate<ProjectTaskId>,
    ISoftDeletableEntity,
    IProjectTaskUpdater<AbstractProjectTask, UpdateAbstractProjectTaskProperties>
{
    protected AbstractProjectTask(ProjectTaskId id) : base(id)
    {
    }

    public DateTime DeletedOnUtc { get; }
    public bool Deleted { get; }

    public ProjectTaskName Name { get; private set; }
    public ProjectTaskDescription Description { get; private set; }
    public ProjectTaskPriority Priority { get; private set; }
    public ProjectTaskStatusId StatusId { get; private set; }
    public DateTime? DueDate { get; private set; }
    public UserId OwnerId { get; private set; }
    public UserId? AssigneeId { get; private set; }
    public ProjectId ProjectId { get; private set; }
    public TimeSpan? EstimatedTime { get; private set; }
    public uint? StoryPoints { get; private set; }

    /// <summary>
    /// Edits name of the ProjectTask, and validates new name.
    /// </summary>
    /// <param name="name">New name.</param>
    /// <returns>Ok result with ProjectTaskName object or an error.</returns>
    public Result<ProjectTaskName> EditName(string name)
    {
        var result = ProjectTaskName.Create(name);
        if (result.IsFailed)
        {
            return result;
        }

        Name = result.Value;

        var evt = new ProjectTaskNameEditedEvent(result.Value, Id);
        Raise(evt);

        return result;
    }

    /// <summary>
    /// Edits description of the ProjectTask, and validates new description.
    /// </summary>
    /// <param name="description">New description.</param>
    /// <returns>Ok result with ProjectTaskDescription object or an error.</returns>
    public Result<ProjectTaskDescription> EditDescription(string description)
    {
        var result = ProjectTaskDescription.Create(description);
        if (result.IsFailed)
        {
            return result;
        }

        Description = result.Value;

        var evt = new ProjectTaskDescriptionEditedEvent(result.Value, Id);
        Raise(evt);

        return result;
    }

    /// <summary>
    /// Changes ProjectTask priority to one of <see cref="ProjectTaskPriority"/>
    /// </summary>
    /// <param name="priority">New priority.</param>
    /// <returns>Ok result with ProjectTaskPriority object.</returns>
    public Result<ProjectTaskPriority> ChangePriority(ProjectTaskPriority priority)
    {
        Priority = priority;

        var evt = new ProjectTaskPriorityChangedEvent(priority, Id);
        Raise(evt);

        return Result.Ok(priority);
    }

    /// <summary>
    /// Changes ProjectTask status id to one of Project defined or default statuses.
    /// </summary>
    /// <param name="statusId">New status id.</param>
    /// <returns>Ok result with ProjectTaskStatusId object.</returns>
    public Result<ProjectTaskStatusId> ChangeStatus(ProjectTaskStatusId statusId)
    {
        StatusId = statusId;

        var evt = new ProjectTaskStatusIdChangedEvent(statusId, Id);
        Raise(evt);

        return Result.Ok(statusId);
    }

    /// <summary>
    /// Changes ProjectTask dueDate.
    /// </summary>
    /// <param name="dueDate">New dueDate.</param>
    /// <returns>Ok result with DateTime object.</returns>
    public Result<DateTime?> SetDueDate(DateTime? dueDate)
    {
        DueDate = dueDate;

        var evt = new ProjectTaskDueDateSetEvent(dueDate, Id);
        Raise(evt);

        return Result.Ok(dueDate);
    }

    /// <summary>
    /// Changes ProjectTask assignee.
    /// </summary>
    /// <param name="assigneeId">New assigneeId.</param>
    /// <returns>Ok result with UserId object.</returns>
    public Result<UserId?> AssignUser(UserId? assigneeId)
    {
        AssigneeId = assigneeId;

        var evt = new ProjectTaskUserAssignedEvent(assigneeId, Id);
        Raise(evt);

        return Result.Ok(assigneeId);
    }

    /// <summary>
    /// Edits ProjectTask estimated time.
    /// </summary>
    /// <param name="estimatedTime">New estimatedTime.</param>
    /// <returns>Ok result with TimeSpan object.</returns>
    public Result<TimeSpan?> EditEstimatedTime(TimeSpan? estimatedTime)
    {
        EstimatedTime = estimatedTime;

        var evt = new ProjectTaskEstimatedTimeEditedEvent(estimatedTime, Id);
        Raise(evt);

        return Result.Ok(estimatedTime);
    }

    /// <summary>
    /// Edits ProjectTask story points.
    /// </summary>
    /// <param name="storyPoints">New story points.</param>
    /// <returns>Ok result with uint object.</returns>
    public Result<uint?> EditStoryPoints(uint? storyPoints)
    {
        StoryPoints = storyPoints;

        var evt = new ProjectTaskStoryPointsEditedEvent(storyPoints, Id);
        Raise(evt);

        return Result.Ok(storyPoints);
    }

    protected Result<AbstractProjectTask> Create(ProjectTaskProperties properties)
    {
        var resultName = ProjectTaskName.Create(properties.Name);
        var resultDescription = ProjectTaskDescription.Create(properties.Description);
        var priorityParsed = ProjectTaskPriority.TryFromName(properties.Priority, true, out var priority);
        var resultPriority = priorityParsed ? Result.Ok(priority) : Result.Fail(new InvalidPriorityName());

        var result = Result.Merge(resultName, resultDescription, resultPriority);
        if (result.IsFailed)
        {
            return result;
        }

        Name = resultName.Value;
        OwnerId = properties.OwnerId ?? throw new ArgumentException("Owner can't be null");
        Priority = resultPriority.Value;
        StatusId = properties.StatusId;
        ProjectId = properties.ProjectId;
        Description = resultDescription.Value;
        EstimatedTime = properties.EstimatedTime;
        DueDate = properties.DueDate;
        StoryPoints = properties.StoryPoints;
        AssigneeId = properties.AssigneeId;

        return Result.Ok(this);
    }

    public Result<AbstractProjectTask> Update(UpdateAbstractProjectTaskProperties properties)
    {
        var results = new List<Result>();

        if (properties.Name.ShouldUpdate)
        {
            results.Add(EditName(properties.Name.Value!).ToResult());
        }

        if (properties.Priority.ShouldUpdate)
        {
            var priorityParsed = ProjectTaskPriority.TryFromName(properties.Priority.Value, true, out var priority);
            var resultPriority = priorityParsed ? Result.Ok(priority) : Result.Fail(new InvalidPriorityName());
            ChangePriority(priority);
            results.Add(resultPriority.ToResult());
        }

        if (properties.StatusId.ShouldUpdate)
        {
            results.Add(ChangeStatus(properties.StatusId.Value).ToResult());
        }

        if (properties.Description.ShouldUpdate)
        {
            results.Add(EditDescription(properties.Description.Value!).ToResult());
        }

        if (properties.EstimatedTime.ShouldUpdate)
        {
            results.Add(EditEstimatedTime(properties.EstimatedTime.Value).ToResult());
        }

        if (properties.DueDate.ShouldUpdate)
        {
            results.Add(SetDueDate(properties.DueDate.Value).ToResult());
        }

        if (properties.StoryPoints.ShouldUpdate)
        {
            results.Add(EditStoryPoints(properties.StoryPoints.Value).ToResult());
        }

        if (properties.AssigneeId.ShouldUpdate)
        {
            results.Add(AssignUser(properties.AssigneeId.Value).ToResult());
        }

        var result = Result.Merge(results.ToArray());
        if (result.IsFailed)
        {
            return result;
        }

        return Result.Ok();
    }
}