using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after adding new status to project.
/// </summary>
public class ProjectTaskStatusAddedToProjectEvent : IDomainEvent
{
    public ProjectTaskStatusAddedToProjectEvent(ProjectTaskStatus projectTaskStatus, ProjectId projectId)
    {
        ProjectTaskStatus = projectTaskStatus;
        ProjectId = projectId;
    }

    public ProjectTaskStatus ProjectTaskStatus { get; set; }
    public ProjectId ProjectId { get; set; }
}