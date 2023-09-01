using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.ValueObjects;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after Project has been edited.
/// </summary>
public class ProjectEditedEvent : IDomainEvent
{
    public ProjectEditedEvent(ProjectName name, ProjectDescription description, ProjectId projectId)
    {
        Name = name;
        Description = description;
        ProjectId = projectId;
    }

    public ProjectName Name { get; set; }
    public ProjectDescription Description { get; set; }
    public ProjectId ProjectId { get; set; }
}