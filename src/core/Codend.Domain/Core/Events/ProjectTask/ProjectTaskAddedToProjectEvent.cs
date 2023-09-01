using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after adding new task to project.
/// </summary>
public class ProjectTaskAddedToProjectEvent : IDomainEvent
{
    public ProjectTaskAddedToProjectEvent(Entities.ProjectTask projectTask, ProjectId projectId)
    {
        ProjectTask = projectTask;
        ProjectId = projectId;
    }

    public Entities.ProjectTask ProjectTask { get; set; }
    public ProjectId ProjectId { get; set; }
}