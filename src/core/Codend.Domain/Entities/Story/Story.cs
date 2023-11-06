using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;
using FluentResults;

namespace Codend.Domain.Entities;

/// <summary>
/// User story domain model for scrum.
/// </summary>
public class Story : Entity<StoryId>, ISoftDeletableEntity, IProjectOwnedEntity
{
    [Obsolete("Public for 1 unit test 👍🤑🤓", true)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Story() : base(new StoryId(Guid.NewGuid()))
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    private Story(
        ProjectId projectId,
        StoryName name,
        StoryDescription description,
        EpicId? epicId,
        ProjectTaskStatusId statusId) : base(new StoryId(Guid.NewGuid()))
    {
        ProjectId = projectId;
        Name = name;
        Description = description;
        EpicId = epicId;
        StatusId = statusId;
    }

    /// <summary>
    /// String representation of task type. 
    /// </summary>
    public string TaskType => nameof(Story);

    #region ISoftDeletableEntity properties

    /// <inheritdoc />
    public DateTime DeletedOnUtc { get; }

    /// <inheritdoc />
    public bool Deleted { get; }

    #endregion


    #region Story properties

    /// <summary>
    /// ProjectId which story belongs to.
    /// </summary>
    public ProjectId ProjectId { get; private set; }

    /// <summary>
    /// User story name.
    /// </summary>
    public StoryName Name { get; private set; }

    /// <summary>
    /// User story description.
    /// </summary>
    public StoryDescription Description { get; private set; }

    /// <summary>
    /// EpicId which story belongs to.
    /// </summary>
    public EpicId? EpicId { get; private set; }

    /// <summary>
    /// Story status.
    /// </summary>
    public ProjectTaskStatusId StatusId { get; private set; }

    #endregion


    #region Domain methods

    /// <summary>
    /// User story creator.
    /// </summary>
    /// <param name="name">User story name.</param>
    /// <param name="description">User story description.</param>
    /// <param name="projectId">User story project id.</param>
    /// <param name="epicId">User story epic id.</param>
    /// <param name="statusId">Story status id.</param>
    /// <returns>Created <see cref="Story"/> or <see cref="Result"/> with errors.</returns>
    public static Result<Story> Create(
        string name,
        string description,
        ProjectId projectId,
        EpicId? epicId,
        ProjectTaskStatusId statusId)
    {
        var resultName = StoryName.Create(name);
        var resultDescription = StoryDescription.Create(description);

        var result = Result.Merge(resultName, resultDescription);
        if (result.IsFailed)
        {
            return result;
        }

        var story = new Story(projectId, resultName.Value, resultDescription.Value, epicId, statusId);

        return Result.Ok(story);
    }

    /// <summary>
    /// Edits story name.
    /// </summary>
    /// <param name="name">New name.</param>
    /// <returns>Ok <see cref="Result"/> with new story name or failure with errors.</returns>
    public virtual Result<StoryName> EditName(string name)
    {
        var resultName = StoryName.Create(name);
        if (resultName.IsFailed)
        {
            return resultName;
        }

        Name = resultName.Value;
        return resultName;
    }

    /// <summary>
    /// Edit story description.
    /// </summary>
    /// <param name="description">New description.</param>
    /// <returns>Ok <see cref="Result"/> with new story description or failure with errors.</returns>
    public virtual Result<StoryDescription> EditDescription(string description)
    {
        var resultDescription = StoryDescription.Create(description);
        if (resultDescription.IsFailed)
        {
            return resultDescription;
        }

        Description = resultDescription.Value;
        return resultDescription;
    }

    public Result<EpicId?> EditEpicId(EpicId? epicId)
    {
        EpicId = epicId;
        return Result.Ok(EpicId);
    }

    /// <summary>
    /// Changes Story status id to one of Project defined or default statuses.
    /// </summary>
    /// <param name="statusId">New status id.</param>
    /// <returns>Ok result with ProjectTaskStatusId object.</returns>
    public Result<ProjectTaskStatusId> EditStatus(ProjectTaskStatusId statusId)
    {
        StatusId = statusId;

        return Result.Ok(statusId);
    }

    #endregion
}