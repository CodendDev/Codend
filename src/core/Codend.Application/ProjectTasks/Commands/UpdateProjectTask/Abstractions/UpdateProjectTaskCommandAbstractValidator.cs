using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.Extensions;
using Codend.Domain.Core.Enums;
using Codend.Domain.Entities.ProjectTask.Abstractions;
using Codend.Domain.ValueObjects;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;

/// <summary>
/// Abstract validator for <typeparamref name="TUpdateProjectTaskCommand"/>.
/// </summary>
public abstract class UpdateProjectTaskCommandAbstractValidator<TUpdateProjectTaskCommand>
    : AbstractValidator<TUpdateProjectTaskCommand>
    where TUpdateProjectTaskCommand : ICommand, IUpdateProjectTaskCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProjectTaskCommandAbstractValidator{TUpdateProjectTaskCommand}"/> class.
    /// </summary>
    protected UpdateProjectTaskCommandAbstractValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(IUpdateProjectTaskCommand.TaskId)));

        When(x => x.Name is not null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithError(new PropertyNullOrEmpty(nameof(IUpdateProjectTaskCommand.Name)))
                .MaximumLength(ProjectTaskName.MaxLength)
                .WithError(new StringPropertyTooLong(nameof(IUpdateProjectTaskCommand.Name),
                    ProjectTaskName.MaxLength));
        });

        When(x => x.Priority is not null, () =>
        {
            RuleFor(x => x.Priority)
                .NotEmpty()
                .WithError(new PropertyNullOrEmpty(nameof(IUpdateProjectTaskCommand.Priority)))
                .Must(x => ProjectTaskPriority.TryFromName(x, true, out _))
                .WithError(new ProjectTask.PriorityNotDefined());
        });

        RuleFor(x => x.StatusId)
            .MustNotBeDefaultGuid()
            .WithError(new PropertyNullOrEmpty(nameof(IUpdateProjectTaskCommand.StatusId)));

        When(x => x.Description.ShouldUpdate, () =>
        {
            RuleFor(x => x.Description.Value)
                .MaximumLength(ProjectTaskDescription.MaxLength)
                .WithError(new StringPropertyTooLong(nameof(IUpdateProjectTaskCommand.Description),
                    ProjectTaskDescription.MaxLength));
        });

        When(x => x.DueDate.ShouldUpdate, () =>
        {
            RuleFor(x => x.DueDate.Value)
                .Must(x => !x.HasValue || x.Value.CompareTo(DateTime.UtcNow) > 0)
                .WithError(new DateIsInThePast(nameof(IProjectTaskCreateProperties.DueDate)));
        });
    }
}