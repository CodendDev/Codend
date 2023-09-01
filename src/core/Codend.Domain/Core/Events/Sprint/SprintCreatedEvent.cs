using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after new sprint has been created.
/// </summary>
public class SprintCreatedEvent : IDomainEvent
{
    public SprintCreatedEvent(Sprint sprint, ProjectId projectId)
    {
        Sprint = sprint;
        ProjectId = projectId;
    }

    public Sprint Sprint { get; set; }
    public ProjectId ProjectId { get; set; }
}