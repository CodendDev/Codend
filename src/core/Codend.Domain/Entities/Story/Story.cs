using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;
using FluentResults;

namespace Codend.Domain.Entities;

/// <summary>
/// User story domain model for scrum.
/// </summary>
public class Story : Entity<StoryId>, ISoftDeletableEntity
{
    [Obsolete("Public for 1 unit test 👍🤑🤓", true)]
    public Story() : base(new StoryId(Guid.NewGuid()))
    {
    }

    private Story(ProjectId projectId, StoryName name, StoryDescription description, EpicId? epicId)
        : base(new StoryId(Guid.NewGuid()))
    {
        ProjectId = projectId;
        Name = name;
        Description = description;
        EpicId = epicId;
    }


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
    public EpicId? EpicId { get; set; }

    #endregion


    #region Domain methods

    /// <summary>
    /// User story creator.
    /// </summary>
    /// <param name="name">User story name.</param>
    /// <param name="description">User story description.</param>
    /// <param name="projectId">User story project id.</param>
    /// <param name="epicId">User story epic id.</param>
    /// <returns>Created <see cref="Story"/> or <see cref="Result"/> with errors.</returns>
    public static Result<Story> Create(string name, string description, ProjectId projectId, EpicId? epicId)
    {
        var resultName = StoryName.Create(name);
        var resultDescription = StoryDescription.Create(description);

        var result = Result.Merge(resultName, resultDescription);
        if (result.IsFailed)
        {
            return result;
        }

        var story = new Story(projectId, resultName.Value, resultDescription.Value, epicId);

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

    public Result EditEpicId(EpicId? epicId)
    {
        EpicId = epicId;
        return Result.Ok();
    }

    #endregion
}