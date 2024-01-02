using Codend.Application.Extensions;
using Codend.Domain.ValueObjects;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Sprints.Commands.CreateSprint;

/// <summary>
/// Validates <see cref="CreateSprintCommand"/>.
/// </summary>
public class CreateSprintCommandValidator : AbstractValidator<CreateSprintCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSprintCommandValidator"/> class.
    /// </summary>
    public CreateSprintCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(CreateSprintCommand.ProjectId)));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(CreateSprintCommand.Name)))
            .MaximumLength(SprintName.MaxLength)
            .WithError(new StringPropertyTooLong(nameof(CreateSprintCommand.Name), SprintName.MaxLength));

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(CreateSprintCommand.StartDate)));

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(CreateSprintCommand.EndDate)));

        When(x => x.Goal is not null, () =>
        {
            RuleFor(x => x.Goal)
                .NotEmpty()
                .WithError(new PropertyNullOrEmpty(nameof(CreateSprintCommand.Goal)));
        });
    }
}