using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after adding new user to project.
/// </summary>
public class UserAddedToProjectEvent : IDomainEvent
{
    public UserAddedToProjectEvent(User user, ProjectId projectId)
    {
        User = user;
        ProjectId = projectId;
    }
    public User User { get; set; }
    public ProjectId ProjectId { get; set; }
}