using Codend.Application.Extensions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

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
            .WithError(new PropertyNullOrEmpty(nameof(EnableUserNotificationsCommand.ProjectId)));
    }
}