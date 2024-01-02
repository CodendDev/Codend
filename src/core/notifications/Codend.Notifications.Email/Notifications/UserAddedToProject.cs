using Codend.Application.Core.Abstractions.Services;
using Codend.Contracts.Responses;
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

    protected override string GetEmailSubject(UserAddedToProjectEvent notification) =>
        "[Codend] You have been assigned to new task!📝";

    protected override string GetEmailMessage(UserAddedToProjectEvent notification, UserDetails receiver) =>
        $"Hello {receiver.FirstName},\n\nYou've been added to a new project. Please take a moment to review the project details and familiarize yourself with the ongoing tasks. \n\nBest regards, \n Codend";
}