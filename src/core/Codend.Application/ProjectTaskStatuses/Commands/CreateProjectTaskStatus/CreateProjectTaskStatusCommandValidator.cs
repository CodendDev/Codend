using Codend.Application.Extensions;
using Codend.Domain.ValueObjects;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.ProjectTaskStatuses.Commands.CreateProjectTaskStatus;

/// <summary>
/// <see cref="CreateProjectTaskStatusCommand"/> validator.
/// </summary>
public class CreateProjectTaskStatusCommandValidator : AbstractValidator<CreateProjectTaskStatusCommand>
{
    /// <summary>
    /// Initializes validation rules for <see cref="CreateProjectTaskStatusCommand"/>.
    /// </summary>
    public CreateProjectTaskStatusCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(CreateProjectTaskStatusCommand.ProjectId)));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(CreateProjectTaskStatusCommand.Name)))
            .MaximumLength(ProjectTaskStatusName.MaxLength)
            .WithError(new StringPropertyTooLong(nameof(CreateProjectTaskStatusCommand.Name), ProjectTaskStatusName.MaxLength));
    }
}