namespace Codend.Notifications.Email.Abstractions;

public interface IEmailService
{
    Task SendAsync(EmailNotification message, CancellationToken cancellationToken);
}