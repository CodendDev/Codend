using Codend.Application.Core.Abstractions.Services;
using Codend.Application.Core.Notifications.Core;
using Codend.Contracts.Responses;
using Codend.Domain.Core.Abstractions;
using Codend.Notifications.Email.Abstractions;

namespace Codend.Notifications.Email.Notifications.Core;

public abstract class UserEmailNotificationAbstractHandler<TNotification>
    : UserNotificationAbstractHandler<TNotification, IEmailService, EmailNotification>
    where TNotification : class, IUserNotification
{
    private readonly IUserService _userService;

    protected UserEmailNotificationAbstractHandler(IEmailService notificationService, IUserService userService)
        : base(notificationService)
    {
        _userService = userService;
    }

    protected abstract string GetEmailSubject(TNotification notification);
    protected abstract string GetEmailMessage(TNotification notification, UserDetails receiver);

    protected override async Task<EmailNotification> GetMessageAsync(TNotification notification)
    {
        var receiver = await _userService.GetUserDetails(notification.Receiver);
        var subject = GetEmailSubject(notification);
        var message = GetEmailMessage(notification, receiver);
        return new EmailNotification(receiver.Email, subject, message);
    }
}