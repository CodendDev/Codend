using Codend.Application.Core.Abstractions.Services;
using Codend.Domain.Core.Events;
using Codend.Notifications.Email.Abstractions;
using Codend.Notifications.Email.Notifications.Core;

namespace Codend.Notifications.Email.Notifications;

public class ProjectTaskUserAssigned : UserEmailNotificationAbstractHandler<ProjectTaskUserAssignedEvent>
{
    public ProjectTaskUserAssigned(IEmailService notificationService, IUserService userService)
        : base(notificationService, userService)
    {
    }

    protected override string GetEmailSubject(ProjectTaskUserAssignedEvent notification) => "dodany";

    protected override string GetEmailMessage(ProjectTaskUserAssignedEvent notification) => "do zadania 💀";
}