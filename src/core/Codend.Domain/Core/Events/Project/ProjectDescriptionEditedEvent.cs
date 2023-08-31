using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.ValueObjects;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after ProjectDescription has been changed
/// </summary>
public class ProjectDescriptionEditedEvent : IDomainEvent
{
    public ProjectDescriptionEditedEvent(ProjectDescription description, ProjectId projectId)
    {
        Description = description;
        ProjectId = projectId;
    }

    public ProjectDescription Description { get; set; }
    public ProjectId ProjectId { get; set; }
}