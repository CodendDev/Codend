using Codend.Application.Core.Abstractions.Services;
using Codend.Domain.Core.Events;
using Codend.Notifications.Email.Abstractions;
using Codend.Notifications.Email.Notifications.Core;

namespace Codend.Notifications.Email.Notifications;

public class UserAddedToProject : UserEmailNotificationAbstractHandler<UserAddedToProjectEvent>
{
    public UserAddedToProject(IEmailService notificationService, IUserService userService)
        : base(notificationService, userService)
    {
    }

    protected override string GetEmailSubject(UserAddedToProjectEvent notification) => "dodany";

    protected override string GetEmailMessage(UserAddedToProjectEvent notification) => "do projektu 💀";
}