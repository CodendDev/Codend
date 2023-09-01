using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after ProjectTask has been added to sprint.
/// </summary>
public class ProjectTaskAddedToSprintEvent : IDomainEvent
{
    public ProjectTaskAddedToSprintEvent(Sprint sprint, ProjectTask projectTask)
    {
        Sprint = sprint;
        ProjectTask = projectTask;
    }

    public Sprint Sprint { get; set; }
    public ProjectTask ProjectTask { get; set; }
}