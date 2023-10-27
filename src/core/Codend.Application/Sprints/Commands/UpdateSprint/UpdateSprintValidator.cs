using Codend.Application.Extensions;
using Codend.Domain.ValueObjects;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Sprints.Commands.UpdateSprint;

/// <summary>
/// Validates <see cref="UpdateSprintCommand"/>.
/// </summary>
public class UpdateSprintValidator : AbstractValidator<UpdateSprintCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSprintValidator"/> class.
    /// </summary>
    public UpdateSprintValidator()
    {
        RuleFor(x => x.SprintId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(UpdateSprintCommand.SprintId)));
        
        When(x => x.Name is not null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithError(new PropertyNullOrEmpty(nameof(UpdateSprintCommand.Name)))
                .MaximumLength(SprintName.MaxLength)
                .WithError(new StringPropertyTooLong(nameof(UpdateSprintCommand.Name), StoryName.MaxLength));
        });
        
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(UpdateSprintCommand.StartDate)));

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(UpdateSprintCommand.EndDate)));
        
        When(x => x.Goal.ShouldUpdate, () =>
        {
            RuleFor(x => x.Goal.Value)
                .NotEmpty()
                .WithError(new PropertyNullOrEmpty(nameof(UpdateSprintCommand.Goal)));
        });
    }
}