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
    DomainEventsAggregate<ProjectTaskId>,
    ISoftDeletableEntity,
    IProjectTaskCreator<BaseProjectTask, BaseProjectTaskCreateProperties>,
    IProjectOwnedEntity,
    ISprintTask
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected BaseProjectTask() : base(new ProjectTaskId(Guid.NewGuid()))
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    /// <summary>
    /// String representation of task type. 
    /// </summary>
    public string TaskType => GetType().Name.Replace("ProjectTask", "");

    #region ISprintTask properties

    public Guid SprintTaskId => Id.Value;
    public string SprintTaskType => TaskType;
    public string SprintTaskName => Name.Value;
    public Guid SprintTaskStatusId => StatusId.Value;
    public string? SprintTaskPriority => Priority.Name;
    public Guid? SprintTaskRelatedTaskId => StoryId?.Value;

    #endregion

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
    public StoryId? StoryId { get; private set; }

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

        return result;
    }

    /// <summary>
    /// Edits description of the ProjectTask, and validates new description.
    /// </summary>
    /// <param name="description">New description.</param>
    /// <returns>Ok result with ProjectTaskDescription object or an error.</returns>
    public Result<ProjectTaskDescription> EditDescription(string? description)
    {
        var result = ProjectTaskDescription.Create(description);
        if (result.IsFailed)
        {
            return result;
        }

        Description = result.Value;

        return result;
    }

    /// <summary>
    /// Changes ProjectTask priority to one of <see cref="ProjectTaskPriority"/>
    /// </summary>
    /// <param name="priority">New priority.</param>
    /// <returns>Ok result with ProjectTaskPriority object.</returns>
    public Result<ProjectTaskPriority> EditPriority(string priority)
    {
        var priorityResult = ProjectTaskPriority.ParseFromString(priority);
        if (priorityResult.IsSuccess)
        {
            Priority = priorityResult.Value;
        }

        return Result.Ok(Priority);
    }

    /// <summary>
    /// Changes ProjectTask status id to one of Project defined or default statuses.
    /// </summary>
    /// <param name="statusId">New status id.</param>
    /// <returns>Ok result with ProjectTaskStatusId object.</returns>
    public Result<ProjectTaskStatusId> EditStatus(ProjectTaskStatusId statusId)
    {
        StatusId = statusId;

        return Result.Ok(statusId);
    }

    /// <summary>
    /// Changes ProjectTask dueDate.
    /// </summary>
    /// <param name="dueDate">New dueDate.</param>
    /// <returns>Ok result with DateTime object.</returns>
    public Result<DateTime?> EditDueDate(DateTime? dueDate)
    {
        DueDate = dueDate;

        return Result.Ok(dueDate);
    }


    /// <summary>
    /// Assigns a user as the assignee of a project task.
    /// </summary>
    /// <param name="assigner">The user that performs the operation.</param>
    /// <param name="assignee">The member to be assigned to the task, can be null.</param>
    /// <returns>
    /// A <see cref="Result"/> object that encapsulates the result of the operation.
    /// Contains the UserId of the assignee if successful.
    /// </returns>
    public Result<UserId?> AssignUser(ProjectMember assigner, ProjectMember? assignee)
    {
        AssigneeId = assignee?.MemberId;

        // Don't raise event if assigner and assignee is same user or assignee is null
        if (assignee is null || assigner.UserId == assignee.UserId)
        {
            return Result.Ok(AssigneeId);
        }

        var evt = new ProjectTaskUserAssignedEvent(Id, assigner, assignee);
        Raise(evt);
        return Result.Ok(AssigneeId);
    }

    /// <summary>
    /// Edits ProjectTask estimated time.
    /// </summary>
    /// <param name="estimatedTime">New estimatedTime.</param>
    /// <returns>Ok result with TimeSpan object.</returns>
    public Result<TimeSpan?> EditEstimatedTime(TimeSpan? estimatedTime)
    {
        EstimatedTime = estimatedTime;

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

        return Result.Ok(storyPoints);
    }

    /// <summary>
    /// Edits Story to which task belongs.
    /// </summary>
    /// <param name="storyId">New story Id.</param>
    public Result<StoryId?> EditStory(StoryId? storyId)
    {
        StoryId = storyId;
        return Result.Ok(storyId);
    }

    /// <summary>
    /// Creates <see cref="BaseProjectTask"/>.
    /// </summary>
    /// <param name="properties"><see cref="BaseProjectTaskCreateProperties"/> used for creation.</param>
    /// <param name="ownerId">Owner of the task.</param>
    /// <returns>Created <see cref="BaseProjectTask"/> or error.</returns>
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
        StatusId = properties.StatusId!;
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