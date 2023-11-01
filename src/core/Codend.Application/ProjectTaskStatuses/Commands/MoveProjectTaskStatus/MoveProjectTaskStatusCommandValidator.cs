using Codend.Application.Extensions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.ProjectTaskStatuses.Commands.MoveProjectTaskStatus;

/// <summary>
/// Validates <see cref="MoveProjectTaskStatusCommand"/>.
/// </summary>
public class MoveProjectTaskStatusCommandValidator : AbstractValidator<MoveProjectTaskStatusCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MoveProjectTaskStatusCommandValidator"/> class.
    /// </summary>
    public MoveProjectTaskStatusCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(MoveProjectTaskStatusCommand.ProjectId)));

        RuleFor(x => x.StatusId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(MoveProjectTaskStatusCommand.StatusId)));

        When(x => x.Prev is not null, () =>
        {
            RuleFor(x => x.Prev)
                .NotEmpty()
                .WithError(new PropertyNullOrEmpty(nameof(MoveProjectTaskStatusCommand.Prev)));
        });

        When(x => x.Next is not null, () =>
        {
            RuleFor(x => x.Next)
                .NotEmpty()
                .WithError(new PropertyNullOrEmpty(nameof(MoveProjectTaskStatusCommand.Next)));
        });
    }
}