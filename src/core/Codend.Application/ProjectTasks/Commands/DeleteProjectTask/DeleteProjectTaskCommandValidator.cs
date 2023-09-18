using Codend.Application.Core.Errors;
using Codend.Application.Extensions;
using FluentValidation;

namespace Codend.Application.ProjectTasks.Commands.DeleteProjectTask;

/// <summary>
/// Validator for <see cref="DeleteProjectTaskCommand"/>.
/// </summary>
public class DeleteProjectTaskCommandValidator : AbstractValidator<DeleteProjectTaskCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProjectTaskCommandValidator"/> class.
    /// </summary>
    public DeleteProjectTaskCommandValidator()
    {
        RuleFor(x => x.ProjectTaskId)
            .NotEmpty()
            .WithError(new ValidationErrors.Common.PropertyNullOrEmpty(nameof(DeleteProjectTaskCommand.ProjectTaskId)));
    }
}