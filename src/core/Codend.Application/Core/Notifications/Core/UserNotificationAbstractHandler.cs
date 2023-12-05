using Codend.Application.Core.Notifications.Abstractions;
using Codend.Domain.Core.Abstractions;
using MediatR;

namespace Codend.Application.Core.Notifications.Core;

/// <summary>
/// Abstract <see cref="IUserNotification"/> handler. Calls all <see cref="IUserNotificationService{T}"/> services.
/// </summary>
/// <typeparam name="TNotificationService">Notification service.</typeparam>
/// <typeparam name="TNotificationEvent"><see cref="IUserNotification"/> notification.</typeparam>
/// <typeparam name="TNotificationMessage">Notification message type.</typeparam>
public abstract class UserNotificationAbstractHandler<TNotificationEvent, TNotificationService, TNotificationMessage>
    : INotificationHandler<TNotificationEvent>
    where TNotificationService : IUserNotificationService<TNotificationMessage>
    where TNotificationEvent : class, IUserNotification
    where TNotificationMessage : class, INotificationMessage
{
    /// <summary>
    /// User notification services.
    /// </summary>
    private readonly TNotificationService _notificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotificationAbstractHandler{T, TService, TMessage}"/> class.
    /// </summary>
    protected UserNotificationAbstractHandler(TNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <inheritdoc />
    public virtual async Task Handle(TNotificationEvent notification, CancellationToken cancellationToken) =>
        await _notificationService.SendNotificationAsync(await GetMessageAsync(notification), cancellationToken);

    /// <summary>
    /// Generates notification message.
    /// </summary>
    /// <param name="notification">Notification data.</param>
    /// <returns>Notification message.</returns>
    protected abstract Task<TNotificationMessage> GetMessageAsync(TNotificationEvent notification);
}