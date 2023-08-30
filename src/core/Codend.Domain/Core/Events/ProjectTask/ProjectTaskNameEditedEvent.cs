using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.ValueObjects;

namespace Codend.Domain.Core.Events.ProjectTask;

/// <summary>
/// Domain event raised after ProjectTaskName has been changed
/// </summary>
public class ProjectTaskNameEditedEvent : IDomainEvent
{
    public ProjectTaskNameEditedEvent(ProjectTaskName name, ProjectTaskId projectTaskId)
    {
        Name = name;
        ProjectTaskId = projectTaskId;
    }

    public ProjectTaskName Name { get; set; }
    public ProjectTaskId ProjectTaskId { get; set; }
}