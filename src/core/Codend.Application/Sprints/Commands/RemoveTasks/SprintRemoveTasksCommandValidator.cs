using Codend.Application.Extensions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Sprints.Commands.RemoveTasks;

/// <summary>
/// Validates <see cref="SprintRemoveTasksCommand"/>.
/// </summary>
public class SprintRemoveTasksCommandValidator : AbstractValidator<SprintRemoveTasksCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SprintRemoveTasksCommandValidator"/> class.
    /// </summary>
    public SprintRemoveTasksCommandValidator()
    {
        RuleFor(x => x.SprintId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(SprintRemoveTasksCommand.SprintId)));

        RuleFor(x => x.TasksIds)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(SprintRemoveTasksCommand.TasksIds)));
    }
}