using Codend.Application.Extensions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.ProjectTaskStatuses.Commands.DeleteProjectTaskStatus;

/// <summary>
/// Validates <see cref="DeleteProjectTaskStatusCommand"/>.
/// </summary>
public class DeleteProjectTaskStatusCommandValidator : AbstractValidator<DeleteProjectTaskStatusCommand>
{
    /// <summary>
    /// Initializes validation rules for <see cref="DeleteProjectTaskStatusCommand"/>.
    /// </summary>
    public DeleteProjectTaskStatusCommandValidator()
    {
        RuleFor(x => x.StatusId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(DeleteProjectTaskStatusCommand.StatusId)));
    }
}