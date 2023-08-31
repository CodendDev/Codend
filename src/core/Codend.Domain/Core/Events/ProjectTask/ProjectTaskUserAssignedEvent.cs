using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events.ProjectTask;

/// <summary>
/// Domain event raised after AssigneeId has been changed.
/// </summary>
public class ProjectTaskUserAssignedEvent : IDomainEvent
{
    public ProjectTaskUserAssignedEvent(UserId assigneeId, ProjectTaskId projectTaskId)
    {
        AssigneeId = assigneeId;
        ProjectTaskId = projectTaskId;
    }

    public UserId AssigneeId { get; set; }
    public ProjectTaskId ProjectTaskId { get; set; }
}