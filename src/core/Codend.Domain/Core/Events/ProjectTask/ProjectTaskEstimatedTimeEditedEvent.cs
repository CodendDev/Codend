using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after ProjectTask estimatedTime has been changed.
/// </summary>
public class ProjectTaskEstimatedTimeEditedEvent : IDomainEvent
{
    public ProjectTaskEstimatedTimeEditedEvent(TimeSpan? estimatedTime, ProjectTaskId projectTaskId)
    {
        this.EstimatedTime = estimatedTime;
        ProjectTaskId = projectTaskId;
    }

    public TimeSpan? EstimatedTime { get; set; }
    public ProjectTaskId ProjectTaskId { get; set; }
}