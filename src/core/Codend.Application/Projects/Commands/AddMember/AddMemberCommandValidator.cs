using Codend.Application.Core.Errors;
using Codend.Application.Extensions;
using FluentValidation;

namespace Codend.Application.Projects.Commands.AddMember;

/// <summary>
/// <see cref="AddMemberCommand"/> validator.
/// </summary>
public class AddMemberCommandValidator : AbstractValidator<AddMemberCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddMemberCommandValidator"/> class.
    /// </summary>
    public AddMemberCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new ValidationErrors.Common.PropertyNullOrEmpty(nameof(AddMemberCommand.ProjectId)));

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithError(new ValidationErrors.EmailAddress.NotValid());
    }
}