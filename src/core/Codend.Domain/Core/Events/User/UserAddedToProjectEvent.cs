using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after adding new user to project.
/// </summary>
public class UserAddedToProjectEvent : IDomainEvent
{
    public UserAddedToProjectEvent(UserId user, ProjectId projectId)
    {
        User = user;
        ProjectId = projectId;
    }
    public UserId User { get; set; }
    public ProjectId ProjectId { get; set; }
}