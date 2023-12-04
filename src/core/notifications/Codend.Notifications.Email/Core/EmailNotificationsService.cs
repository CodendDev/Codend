using Codend.Application.Core.Notifications.Abstractions;
using Codend.Domain.Core.Abstractions;

namespace Codend.Notifications.Email.Core;

public class EmailNotificationsService : IUserNotificationService<EmailNotification>
{
    public Task SendNotification(IUser user, EmailNotification message)
    {
        // todo
        throw new NotImplementedException();
    }
}