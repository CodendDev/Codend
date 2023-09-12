using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after ProjectTask DueDate has been changed.
/// </summary>
public class ProjectTaskDueDateSetEvent : IDomainEvent
{
    public ProjectTaskDueDateSetEvent(DateTime? dueDate, ProjectTaskId projectTaskId)
    {
        DueDate = dueDate;
        ProjectTaskId = projectTaskId;
    }

    public DateTime? DueDate { get; set; }
    public ProjectTaskId ProjectTaskId { get; set; }
}