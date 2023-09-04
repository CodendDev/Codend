using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after removing user from project.
/// </summary>
public class UserRemovedFromProjectEvent : IDomainEvent
{
    public UserRemovedFromProjectEvent(UserId user, ProjectId projectId)
    {
        User = user;
        ProjectId = projectId;
    }
    
    public UserId User { get; set; }
    public ProjectId ProjectId { get; set; }
}