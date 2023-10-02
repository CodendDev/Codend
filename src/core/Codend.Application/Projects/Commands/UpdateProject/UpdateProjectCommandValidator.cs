using Codend.Application.Extensions;
using Codend.Domain.ValueObjects;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Projects.Commands.UpdateProject;

/// <summary>
/// Validates <see cref="UpdateProjectCommand"/>.
/// </summary>
public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProjectCommandValidator"/> class.
    /// </summary>
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(UpdateProjectCommand.ProjectId)));

        When(x => x.Name is not null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithError(new PropertyNullOrEmpty(nameof(UpdateProjectCommand.Name)))
                .MaximumLength(ProjectName.MaxLength)
                .WithError(new StringPropertyTooLong(nameof(UpdateProjectCommand.Name),
                    ProjectName.MaxLength));
        });

        When(x => x.Description.ShouldUpdate, () =>
        {
            RuleFor(x => x.Description.Value)
                .MaximumLength(ProjectDescription.MaxLength)
                .WithError(new StringPropertyTooLong(nameof(UpdateProjectCommand.Description),
                    ProjectDescription.MaxLength));
        });
    }
}