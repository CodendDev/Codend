using Codend.Application.Core.Notifications.Abstractions;

namespace Codend.Notifications.Email.Abstractions;

public record EmailNotification(
    string Receiver,
    string Subject,
    string Message
) : INotificationMessage;