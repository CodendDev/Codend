using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Core.Events;

/// <summary>
/// Domain event raised after adding new user to project.
/// </summary>
public record UserAddedToProjectEvent
(
    ProjectId ProjectId,
    IUser Receiver
) : IUserNotification;