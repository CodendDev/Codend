using Codend.Application.Core.Notifications.Abstractions;
using Codend.Notifications.Email.Abstractions;

namespace Codend.Notifications.Email.Core;

public class EmailNotificationsService : IUserNotificationService<EmailNotification>
{
    private readonly IEmailService _emailService;

    public EmailNotificationsService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public Task SendNotificationAsync(EmailNotification message, CancellationToken cancellationToken) =>
        _emailService.SendAsync(message, cancellationToken);
}