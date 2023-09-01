using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.ValueObjects;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after deleting project version.
/// </summary>
public class ProjectVersionDeletedEvent : IDomainEvent
{
    public ProjectVersionDeletedEvent(ProjectVersion projectVersion, ProjectId projectId)
    {
        ProjectVersion = projectVersion;
        ProjectId = projectId;
    }

    public ProjectVersion ProjectVersion { get; set; }
    public ProjectId ProjectId { get; set; }
}