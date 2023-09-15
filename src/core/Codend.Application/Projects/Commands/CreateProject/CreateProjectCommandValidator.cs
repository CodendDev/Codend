using Codend.Application.Extensions;
using Codend.Domain.ValueObjects;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Projects.Commands.CreateProject;

/// <summary>
/// Validates <see cref="CreateProjectCommand"/>.
/// </summary>
public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProjectCommandValidator"/> class.
    /// </summary>
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(CreateProjectCommand.Name)))
            .MaximumLength(ProjectName.MaxLength)
            .WithError(new StringPropertyTooLong(nameof(CreateProjectCommand.Name),
                ProjectName.MaxLength));

        RuleFor(x => x.Description)
            .MaximumLength(ProjectDescription.MaxLength)
            .WithError(new StringPropertyTooLong(nameof(CreateProjectCommand.Description),
                ProjectDescription.MaxLength));
    }
}