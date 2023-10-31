using Codend.Application.Extensions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Sprints.Commands.MoveTask;

/// <summary>
/// Validates <see cref="MoveSprintTaskCommand"/>.
/// </summary>
public class MoveSprintTaskCommandValidator : AbstractValidator<MoveSprintTaskCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MoveSprintTaskCommandValidator"/> class.
    /// </summary>
    public MoveSprintTaskCommandValidator()
    {
        RuleFor(x => x.SprintId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(MoveSprintTaskCommand.SprintId)));

        RuleFor(x => x.TaskId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(MoveSprintTaskCommand.TaskId)));

        When(x => x.Prev is not null, () =>
        {
            RuleFor(x => x.Prev)
                .NotEmpty()
                .WithError(new PropertyNullOrEmpty(nameof(MoveSprintTaskCommand.Prev)));
        });

        When(x => x.Next is not null, () =>
        {
            RuleFor(x => x.Next)
                .NotEmpty()
                .WithError(new PropertyNullOrEmpty(nameof(MoveSprintTaskCommand.Next)));
        });
    }
}