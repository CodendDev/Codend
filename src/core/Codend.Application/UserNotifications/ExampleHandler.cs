using Codend.Application.Core.Abstractions.Notifications;
using Codend.Application.UserNotifications;
using Codend.Domain.Core.Events;

namespace Codend.Application.DomainEvents;

public class ExampleHandler : UserNotificationAbstractHandler<UserAddedToProjectEvent, string>
{
    public ExampleHandler(IEnumerable<IUserNotificationService> notificationServices) : base(notificationServices)
    {
    }

    protected override string GetMessage(UserAddedToProjectEvent notification)
    {
        return "Hello";
    }
}