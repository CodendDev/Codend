using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.ValueObjects;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after ProjectName has been changed
/// </summary>
public class ProjectNameEditedEvent : IDomainEvent
{
    public ProjectNameEditedEvent(ProjectName name, ProjectId projectId)
    {
        Name = name;
        ProjectId = projectId;
    }

    public ProjectName Name { get; set; }
    public ProjectId ProjectId { get; set; }
}