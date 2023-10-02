using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.Extensions;
using Codend.Domain.Core.Enums;
using Codend.Domain.Entities.ProjectTask.Abstractions;
using Codend.Domain.ValueObjects;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;

/// <summary>
/// Abstract validator for commands implementing <see cref="ICreateProjectTaskCommand"/> interface.
/// </summary>
public abstract class CreateProjectTaskCommandAbstractValidator<TCreateProjectTaskCommand, TCreateProjectTaskProperties>
    : AbstractValidator<TCreateProjectTaskCommand>
    where TCreateProjectTaskCommand : ICommand<Guid>, ICreateProjectTaskCommand<TCreateProjectTaskProperties>
    where TCreateProjectTaskProperties : IProjectTaskCreateProperties
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProjectTaskCommandAbstractValidator{TCreateProjectTaskCommand,TCreateProjectTaskProperties}"/> class.
    /// </summary>
    protected CreateProjectTaskCommandAbstractValidator()
    {
        RuleFor(x => x.TaskProperties.ProjectId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(IProjectTaskCreateProperties.ProjectId)));

        RuleFor(x => x.TaskProperties.Name)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(IProjectTaskCreateProperties.Name)))
            .MaximumLength(ProjectTaskName.MaxLength)
            .WithError(new StringPropertyTooLong(nameof(IProjectTaskCreateProperties.Name),
                ProjectTaskName.MaxLength));

        RuleFor(x => x.TaskProperties.Priority)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(IProjectTaskCreateProperties.Priority)))
            .Must(x => ProjectTaskPriority.TryFromName(x, true, out _))
            .WithError(new ProjectTask.PriorityNotDefined());

        RuleFor(x => x.TaskProperties.Description)
            .MaximumLength(ProjectTaskDescription.MaxLength)
            .WithError(new StringPropertyTooLong(nameof(IProjectTaskCreateProperties.Description),
                ProjectTaskDescription.MaxLength));

        RuleFor(x => x.TaskProperties.DueDate)
            .Must(x => !x.HasValue || x.Value.CompareTo(DateTime.UtcNow) > 0)
            .WithError(new DateIsInThePast(nameof(IProjectTaskCreateProperties.DueDate)));
    }
}