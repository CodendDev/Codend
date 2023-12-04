using Codend.Domain.Core.Abstractions;

namespace Codend.Application.Core.Notifications.Abstractions;

/// <summary>
/// User notification service.
/// </summary>
public interface IUserNotificationService<in TMessage>
    where TMessage : INotificationMessage
{
    /// <summary>
    /// Sends notification to given <paramref name="user"/>.
    /// </summary>
    /// <param name="user">User data.</param>
    /// <param name="message">Message which will be send to the user.</param>
    /// <returns></returns>
    Task SendNotification(IUser user, TMessage message);
}