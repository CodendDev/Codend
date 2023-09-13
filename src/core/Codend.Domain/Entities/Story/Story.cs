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
    private Story() : base(new StoryId(Guid.NewGuid()))
    {
    }

    private Story(ProjectId projectId, StoryName name, StoryDescription description)
        : base(new StoryId(Guid.NewGuid()))
    {
        ProjectId = projectId;
        Name = name;
        Description = description;
    }

    /// <summary>
    /// ProjectId which story belongs to.
    /// </summary>
    public ProjectId ProjectId { get; set; }

    #region ISoftDeletableEntity properties

    /// <inheritdoc />
    public DateTime DeletedOnUtc { get; }

    /// <inheritdoc />
    public bool Deleted { get; }

    #endregion

    /// <summary>
    /// User story name.
    /// </summary>
    public StoryName Name { get; set; }

    /// <summary>
    /// User story description.
    /// </summary>
    public StoryDescription Description { get; set; }

    /// <summary>
    /// User story creator.
    /// </summary>
    /// <param name="name">User story name.</param>
    /// <param name="description">User story description.</param>
    /// <param name="projectId">User story project id.</param>
    /// <returns>Created <see cref="Story"/> or <see cref="Result"/> with errors.</returns>
    public static Result<Story> Create(string name, string description, ProjectId projectId)
    {
        var resultName = StoryName.Create(name);
        var resultDescription = StoryDescription.Create(description);

        var result = Result.Merge(resultName, resultDescription);
        if (result.IsFailed)
        {
            return result;
        }

        var story = new Story(projectId, resultName.Value, resultDescription.Value);

        return Result.Ok(story);
    }
}