using Codend.Application.Core.Errors;
using Codend.Application.Extensions;
using Codend.Application.Projects.Commands.RemoveMember;
using FluentValidation;

namespace Codend.Application.Projects.Commands.EnableNotifications;

/// <summary>
/// <see cref="EnableUserNotificationsCommand"/> validator.
/// </summary>
public class EnableUserNotificationsCommandValidator : AbstractValidator<EnableUserNotificationsCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnableUserNotificationsCommandValidator"/> class.
    /// </summary>
    public EnableUserNotificationsCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new ValidationErrors.Common.PropertyNullOrEmpty(nameof(RemoveMemberCommand.ProjectId)));
    }
}