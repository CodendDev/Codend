using Codend.Application.Core.Errors;
using Codend.Application.Extensions;
using Codend.Application.Projects.Commands.RemoveMember;
using FluentValidation;

namespace Codend.Application.Projects.Commands.DisableNotifications;

/// <summary>
/// <see cref="DisableUserNotificationsCommand"/> validator.
/// </summary>
public class DisableUserNotificationsCommandValidator : AbstractValidator<DisableUserNotificationsCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DisableUserNotificationsCommandValidator"/> class.
    /// </summary>
    public DisableUserNotificationsCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new ValidationErrors.Common.PropertyNullOrEmpty(nameof(RemoveMemberCommand.ProjectId)));
    }
}