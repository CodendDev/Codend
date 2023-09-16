using Codend.Application.Extensions;
using Codend.Domain.ValueObjects;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Epics.Commands.UpdateEpic;

/// <summary>
/// Validates <see cref="UpdateEpicCommand"/>.
/// </summary>
public class UpdateEpicCommandValidator : AbstractValidator<UpdateEpicCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEpicCommandValidator"/> class.
    /// </summary>
    public UpdateEpicCommandValidator()
    {
        RuleFor(x => x.EpicId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(UpdateEpicCommand.EpicId)));

        When(x => x.Name is not null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithError(new PropertyNullOrEmpty(nameof(UpdateEpicCommand.Name)))
                .MaximumLength(EpicName.MaxLength)
                .WithError(new StringPropertyTooLong(nameof(UpdateEpicCommand.Name), EpicName.MaxLength));
        });

        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithError(new PropertyNullOrEmpty(nameof(UpdateEpicCommand.Name)))
                .MaximumLength(EpicDescription.MaxLength)
                .WithError(new StringPropertyTooLong(nameof(UpdateEpicCommand.Description), EpicDescription.MaxLength));
        });
    }
}