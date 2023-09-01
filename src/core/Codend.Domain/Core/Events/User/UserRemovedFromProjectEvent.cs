using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after removing user from project.
/// </summary>
public class UserRemovedFromProjectEvent : IDomainEvent
{
    public UserRemovedFromProjectEvent(User user, ProjectId projectId)
    {
        User = user;
        ProjectId = projectId;
    }
    
    public User User { get; set; }
    public ProjectId ProjectId { get; set; }
}