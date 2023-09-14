using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Event raised after task story was changed.
/// </summary>
public class ProjectTaskStoryEditedEvent : IDomainEvent
{
    /// <summary>
    /// The TaskId.
    /// </summary>
    public ProjectTaskId ProjectTaskId { get; }

    /// <summary>
    /// New story Id or null.
    /// </summary>
    public StoryId? StoryId { get; }


    /// <param name="projectTaskId">The TaskId.</param>
    /// <param name="storyId">New story Id or null.</param>
    public ProjectTaskStoryEditedEvent(ProjectTaskId projectTaskId, StoryId? storyId)
    {
        ProjectTaskId = projectTaskId;
        StoryId = storyId;
    }
}