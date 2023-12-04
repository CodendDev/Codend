using Codend.Application.Extensions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

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
            .WithError(new PropertyNullOrEmpty(nameof(DisableUserNotificationsCommand.ProjectId)));
    }
}