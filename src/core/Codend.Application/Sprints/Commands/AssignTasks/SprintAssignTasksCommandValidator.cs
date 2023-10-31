using Codend.Application.Core.Errors;
using Codend.Application.Extensions;
using Codend.Domain.Core.Abstractions;
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
            .WithError(new PropertyNullOrEmpty(nameof(SprintAssignTasksCommand.TasksIds)))
            .Must(AllElementsAreUnique)
            .WithError(new ValidationErrors.SprintProjectTask.AllTasksMustBeUnique());
    }

    private static bool AllElementsAreUnique(IEnumerable<ISprintTaskId> tasksIds)
    {
        var sprintTaskIds = tasksIds.ToList();
        return sprintTaskIds.Distinct().Count() == sprintTaskIds.Count;
    }
}