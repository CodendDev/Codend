using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after StoryPoints have been changed.
/// </summary>
public class ProjectTaskStoryPointsEditedEvent : IDomainEvent
{
    public ProjectTaskStoryPointsEditedEvent(uint storyPoints, ProjectTaskId projectTaskId)
    {
        StoryPoints = storyPoints;
        ProjectTaskId = projectTaskId;
    }

    public uint StoryPoints { get; set; }
    public ProjectTaskId ProjectTaskId { get; set; }
}