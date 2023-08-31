using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events.ProjectTask;

/// <summary>
/// Domain event raised after ProjectTaskStatusId has been changed.
/// </summary>
public class ProjectTaskStatusIdChangedEvent : IDomainEvent
{
    public ProjectTaskStatusIdChangedEvent(ProjectTaskStatusId statusId, ProjectTaskId projectTaskId)
    {
        StatusId = statusId;
        ProjectTaskId = projectTaskId;
    }

    public ProjectTaskStatusId StatusId { get; set; }
    public ProjectTaskId ProjectTaskId { get; set; }
}