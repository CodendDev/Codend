using Codend.Application.Core.Abstractions.Notifications;
using Codend.Domain.Core.Abstractions;
using MediatR;

namespace Codend.Application.UserNotifications;

/// <summary>
/// Abstract <see cref="IUserNotification"/> handler. Calls all <see cref="IUserNotificationService"/> services.
/// </summary>
/// <typeparam name="TNotificationEvent"><see cref="IUserNotification"/> notification.</typeparam>
/// <typeparam name="TNotificationMessage">Notification message type.</typeparam>
public abstract class UserNotificationAbstractHandler<TNotificationEvent, TNotificationMessage>
    : INotificationHandler<TNotificationEvent>
    where TNotificationEvent : class, IUserNotification
    where TNotificationMessage : class
{
    /// <summary>
    /// User notification services.
    /// </summary>
    private readonly IEnumerable<IUserNotificationService> _notificationServices;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotificationAbstractHandler{T, TMessage}"/> class.
    /// </summary>
    protected UserNotificationAbstractHandler(IEnumerable<IUserNotificationService> notificationServices)
    {
        _notificationServices = notificationServices;
    }

    /// <inheritdoc />
    public virtual Task Handle(TNotificationEvent notification, CancellationToken cancellationToken)
    {
        var message = GetMessage(notification);
        var tasks = _notificationServices.Select(s =>
            s.SendNotification(notification.User, message)
        );
        return Task.WhenAll(tasks);
    }

    /// <summary>
    /// Generates notification message.
    /// </summary>
    /// <param name="notification">Notification data.</param>
    /// <returns>Notification message.</returns>
    protected abstract TNotificationMessage GetMessage(TNotificationEvent notification);
}