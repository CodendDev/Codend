using Codend.Application.Core.Notifications.Abstractions;

namespace Codend.Notifications.Email.Abstractions;

public interface IEmailService : IUserNotificationService<EmailNotification>
{
}