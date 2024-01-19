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
        "[Codend] You have been added to project!🎉";

    protected override string GetEmailMessage(UserAddedToProjectEvent notification, UserDetails receiver) =>
        $"Hello {receiver.FirstName},\r\n\r\nYou've been added to a new project. Please take a moment to review the project details and familiarize yourself with the ongoing tasks. \r\n\r\nBest regards, \r\n Codend";
}