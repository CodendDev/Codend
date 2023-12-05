using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after AssigneeId has been changed.
/// </summary>
public record ProjectTaskUserAssignedEvent
(
    ProjectTaskId ProjectTaskId,
    IUser Assigner,
    IUser Assignee
) : IUserNotification
{
    public IUser Receiver => Assignee;
}