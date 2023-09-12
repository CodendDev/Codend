using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after Project has been edited.
/// </summary>
public class ProjectEditedEvent : IDomainEvent
{
    public ProjectEditedEvent(Project project)
    {
        Project = project;
    }

    public Project Project { get; set; }
}