using Codend.Domain.Core.Abstractions;

namespace Codend.Application.Core.Notifications.Abstractions;

/// <summary>
/// User notification service.
/// </summary>
public interface IUserNotificationService<in TMessage>
    where TMessage : INotificationMessage
{
    /// <summary>
    /// Sends given notification. 
    /// </summary>
    /// <param name="message">Message which will be send to the user.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    Task SendNotificationAsync(TMessage message, CancellationToken cancellationToken);
}