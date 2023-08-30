using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Enums;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events.ProjectTask;

/// <summary>
/// Domain event raised after ProjectTaskPriority has been changed.
/// </summary>
public class ProjectTaskPriorityChangedEvent : IDomainEvent
{
    public ProjectTaskPriorityChangedEvent(ProjectTaskPriority priority, ProjectTaskId projectTaskId)
    {
        Priority = priority;
        ProjectTaskId = projectTaskId;
    }

    public ProjectTaskPriority Priority { get; set; }
    public ProjectTaskId ProjectTaskId { get; set; }
}