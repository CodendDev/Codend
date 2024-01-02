using Codend.Application.Core.Abstractions.Services;
using Codend.Contracts.Responses;
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

    protected override string GetEmailSubject(ProjectTaskUserAssignedEvent notification) =>
        "[Codend] You have been assigned to new task!📝";

    protected override string GetEmailMessage(ProjectTaskUserAssignedEvent notification, UserDetails receiver) =>
        $"Hi ${receiver.FirstName}!\n\nYou've been assigned to new task, and it might be important 🚨. Please take a moment to check it out. \n\nBest regards, \n Codend";
}