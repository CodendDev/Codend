using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Enums;
using Codend.Domain.Core.Events;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities.ProjectTask.Abstractions;
using Codend.Domain.ValueObjects;
using FluentResults;
using InvalidPriorityName = Codend.Domain.Core.Errors.DomainErrors.ProjectTaskPriority.InvalidPriorityName;

namespace Codend.Domain.Entities;

/// <summary>
/// Abstract base ProjectTask class.
/// </summary>
public class BaseProjectTask :
    Aggregate<ProjectTaskId>,
    ISoftDeletableEntity,
    IProjectTaskCreator<BaseProjectTask, BaseProjectTaskCreateProperties>
{
    protected BaseProjectTask() : base(new ProjectTaskId(Guid.NewGuid()))
    {
    }

    #region ISoftDeletableEntity properties

    public DateTime DeletedOnUtc { get; }
    public bool Deleted { get; }

    #endregion

    #region BaseProjectTask properties

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
    public StoryId? StoryId { get; set; }

    #endregion

    #region Domain methods

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

    /// <summary>
    /// Edits Story to which task belongs.
    /// </summary>
    /// <param name="storyId">New story Id.</param>
    public void EditStory(StoryId? storyId)
    {
        StoryId = storyId;

        var evt = new ProjectTaskStoryEditedEvent(Id, storyId);
        Raise(evt);
    }

    /// <summary>
    /// Creates <see cref="BaseProjectTask"/>.
    /// </summary>
    /// <param name="properties"><see cref="BaseProjectTaskCreateProperties"/> used for creation.</param>
    /// <param name="ownerId">Owner of the task.</param>
    /// <returns>Created &lt;see cref="BaseProjectTask"/&gt; or error.</returns>
    public static Result<BaseProjectTask> Create(BaseProjectTaskCreateProperties properties, UserId ownerId)
    {
        var task = new BaseProjectTask();
        var result = task.PopulateBaseProperties(properties, ownerId);
        if (result.IsFailed)
        {
            return result;
        }

        return Result.Ok(task);
    }

    #endregion

    /// <summary>
    /// Populates properties of base task.
    /// </summary>
    /// <param name="properties"><see cref="IProjectTaskCreateProperties"/> properties.</param>
    /// <param name="ownerId">Owner of the task.</param>
    /// <returns>Created <see cref="BaseProjectTask"/> or error.</returns>
    /// <exception cref="ArgumentException">Throws when OwnerId is null.</exception>
    protected Result<BaseProjectTask> PopulateBaseProperties(IProjectTaskCreateProperties properties, UserId ownerId)
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
        OwnerId = ownerId ?? throw new ArgumentException("Owner can't be null");
        Priority = resultPriority.Value;
        StatusId = properties.StatusId;
        ProjectId = properties.ProjectId;
        Description = resultDescription.Value;
        EstimatedTime = properties.EstimatedTime;
        DueDate = properties.DueDate;
        StoryPoints = properties.StoryPoints;
        AssigneeId = properties.AssigneeId;
        StoryId = properties.StoryId;

        return Result.Ok();
    }
}