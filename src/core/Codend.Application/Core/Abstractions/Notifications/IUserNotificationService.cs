using Codend.Domain.Core.Abstractions;

namespace Codend.Application.Core.Abstractions.Notifications;

/// <summary>
/// User notification service.
/// </summary>
public interface IUserNotificationService
{
    /// <summary>
    /// Notification service name.
    /// </summary>
    string ServiceName { get; }

    /// <summary>
    /// Sends notification to given <paramref name="user"/>.
    /// </summary>
    /// <param name="user">User data.</param>
    /// <param name="message">Message which will be send to the user.</param>
    /// <returns></returns>
    Task SendNotification(IUser user, object message);
}