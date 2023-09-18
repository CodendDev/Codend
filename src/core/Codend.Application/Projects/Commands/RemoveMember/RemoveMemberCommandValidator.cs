using Codend.Application.Core.Errors;
using Codend.Application.Extensions;
using FluentValidation;

namespace Codend.Application.Projects.Commands.RemoveMember;

/// <summary>
/// <see cref="RemoveMemberCommand"/> validator.
/// </summary>
public class RemoveMemberCommandValidator : AbstractValidator<RemoveMemberCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveMemberCommandValidator"/> class.
    /// </summary>
    public RemoveMemberCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new ValidationErrors.Common.PropertyNullOrEmpty(nameof(RemoveMemberCommand.ProjectId)));

        RuleFor(x => x.Userid)
            .NotEmpty()
            .WithError(new ValidationErrors.Common.PropertyNullOrEmpty(nameof(RemoveMemberCommand.Userid)));
    }
}