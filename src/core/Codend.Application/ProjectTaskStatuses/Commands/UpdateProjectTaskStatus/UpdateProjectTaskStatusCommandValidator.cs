using Codend.Application.Extensions;
using Codend.Domain.ValueObjects;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.ProjectTaskStatuses.Commands.UpdateProjectTaskStatus;

/// <summary>
/// Validates <see cref="UpdateProjectTaskStatusCommand"/>.
/// </summary>
public class UpdateProjectTaskStatusCommandValidator : AbstractValidator<UpdateProjectTaskStatusCommand>
{
    /// <summary>
    /// Initializes validation rules for <see cref="UpdateProjectTaskStatusCommand"/>.
    /// </summary>
    public UpdateProjectTaskStatusCommandValidator()
    {
        RuleFor(x => x.StatusId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(UpdateProjectTaskStatusCommand.StatusId)));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(UpdateProjectTaskStatusCommand.Name)))
            .MaximumLength(ProjectTaskStatusName.MaxLength)
            .WithError(new StringPropertyTooLong(nameof(UpdateProjectTaskStatusCommand.Name), ProjectTaskStatusName.MaxLength));
    }
}