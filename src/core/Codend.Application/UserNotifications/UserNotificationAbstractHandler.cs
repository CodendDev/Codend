using Codend.Application.Core.Abstractions.Notifications;
using Codend.Domain.Core.Abstractions;
using MediatR;

namespace Codend.Application.DomainEvents;

/// <summary>
/// Abstract <see cref="IUserNotification"/> handler. Calls all <see cref="IUserNotificationService"/> services.
/// </summary>
/// <typeparam name="T"><see cref="IUserNotification"/> notification.</typeparam>
public abstract class UserNotificationAbstractHandler<T>
    : INotificationHandler<T>
    where T : IUserNotification
{
    /// <summary>
    /// User notification services.
    /// </summary>
    protected readonly IEnumerable<IUserNotificationService> NotificationServices;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotificationAbstractHandler{T}"/> class.
    /// </summary>
    protected UserNotificationAbstractHandler(IEnumerable<IUserNotificationService> notificationServices)
    {
        NotificationServices = notificationServices;
    }

    /// <inheritdoc />
    public virtual Task Handle(T notification, CancellationToken cancellationToken)
    {
        var tasks = NotificationServices.Select(s => s.SendNotification(notification.User));
        return Task.WhenAll(tasks);
    }
}