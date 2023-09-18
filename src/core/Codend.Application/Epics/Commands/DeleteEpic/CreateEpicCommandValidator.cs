using Codend.Application.Extensions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Epics.Commands.DeleteEpic;

/// <summary>
/// Validates <see cref="DeleteEpicCommand"/>.
/// </summary>
public class DeleteEpicCommandValidator : AbstractValidator<DeleteEpicCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteEpicCommandValidator"/> class.
    /// </summary>
    public DeleteEpicCommandValidator()
    {
        RuleFor(x => x.EpicId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(DeleteEpicCommand.EpicId)));
    }
}