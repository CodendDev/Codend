using Codend.Application.Extensions;
using Codend.Domain.ValueObjects;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Epics.Commands.CreateEpic;

/// <summary>
/// Validates <see cref="CreateEpicCommand"/>.
/// </summary>
public class CreateEpicCommandValidator : AbstractValidator<CreateEpicCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEpicCommandValidator"/> class.
    /// </summary>
    public CreateEpicCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(CreateEpicCommand.Name)))
            .MaximumLength(EpicName.MaxLength)
            .WithError(new StringPropertyTooLong(nameof(CreateEpicCommand.Name), EpicName.MaxLength));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(CreateEpicCommand.Name)))
            .MaximumLength(EpicDescription.MaxLength)
            .WithError(new StringPropertyTooLong(nameof(CreateEpicCommand.Description), EpicDescription.MaxLength));
    }
}