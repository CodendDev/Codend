using Codend.Application.Core.Abstractions.Services;
using Codend.Application.Core.Notifications.Core;
using Codend.Domain.Core.Events;
using Codend.Notifications.Email.Abstractions;
using Codend.Notifications.Email.Core;

namespace Codend.Notifications.Email.Notifications;

public class UserAddedToProject
    : UserNotificationAbstractHandler<UserAddedToProjectEvent, EmailNotificationsService, EmailNotification>
{
    private readonly IUserService _userService;

    public UserAddedToProject(
        EmailNotificationsService notificationService, IUserService userService) : base(notificationService)
    {
        _userService = userService;
    }

    protected override async Task<EmailNotification> GetMessageAsync(UserAddedToProjectEvent notification)
    {
        var user = await _userService.GetUserDetails(notification.Receiver);
        return new EmailNotification(user.Email, "dodany", "do projektu 💀");
    }
}