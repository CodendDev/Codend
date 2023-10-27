using Codend.Application.Extensions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Sprints.Commands.DeleteSprint;

/// <summary>
/// Validates <see cref="DeleteSprintCommand"/>.
/// </summary>
public class DeleteSprintCommandValidator : AbstractValidator<DeleteSprintCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSprintCommandValidator"/> class.
    /// </summary>
    public DeleteSprintCommandValidator()
    {
        RuleFor(x => x.SprintId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(DeleteSprintCommand.SprintId)));
    }
}