using Codend.Application.Extensions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Sprints.Commands.AssignTasks;

/// <summary>
/// Validates <see cref="SprintAssignTasksCommand"/>.
/// </summary>
public class SprintAssignTasksCommandValidator : AbstractValidator<SprintAssignTasksCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SprintAssignTasksCommandValidator"/> class.
    /// </summary>
    public SprintAssignTasksCommandValidator()
    {
        RuleFor(x => x.SprintId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(SprintAssignTasksCommand.SprintId)));

        RuleFor(x => x.TasksIds)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(SprintAssignTasksCommand.TasksIds)));
    }
}