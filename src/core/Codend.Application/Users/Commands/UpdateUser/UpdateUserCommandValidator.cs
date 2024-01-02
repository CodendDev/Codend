using Codend.Application.Extensions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Users.Commands.UpdateUser;

/// <summary>
/// Validates <see cref="UpdateUserCommand"/>.
/// </summary>
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommandValidator"/> class.
    /// </summary>
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(UpdateUserCommand.FirstName)))
            .MaximumLength(30)
            .WithError(new StringPropertyTooLong(nameof(UpdateUserCommand.FirstName), 30));

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(UpdateUserCommand.LastName)))
            .MaximumLength(30)
            .WithError(new StringPropertyTooLong(nameof(UpdateUserCommand.LastName), 30));

        RuleFor(x => x.ImageUrl)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(UpdateUserCommand.ImageUrl)));
    }
}