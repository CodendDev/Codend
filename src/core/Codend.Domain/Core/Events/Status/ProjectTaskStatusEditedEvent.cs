using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after editing status.
/// </summary>
public class ProjectTaskStatusEditedEvent : IDomainEvent
{
    public ProjectTaskStatusEditedEvent(ProjectTaskStatus projectTaskStatus, ProjectId projectId)
    {
        ProjectTaskStatus = projectTaskStatus;
        ProjectId = projectId;
    }

    public ProjectTaskStatus ProjectTaskStatus { get; set; }
    public ProjectId ProjectId { get; set; }
}