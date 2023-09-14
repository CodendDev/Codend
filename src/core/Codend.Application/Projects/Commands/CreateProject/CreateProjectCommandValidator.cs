using Codend.Application.Extensions;
using Codend.Domain.ValueObjects;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors;

namespace Codend.Application.Projects.Commands.CreateProject;

/// <summary>
/// Validates create project command.
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
            .WithError(new Common.PropertyNullOrEmpty(nameof(CreateProjectCommand.Name)))
            .MaximumLength(ProjectName.MaxLength)
            .WithError(new Common.StringPropertyTooLong(nameof(CreateProjectCommand.Name),
                ProjectName.MaxLength));
        
        RuleFor(x => x.Description)
            .MaximumLength(ProjectDescription.MaxLength)
            .WithError(new Common.StringPropertyTooLong(nameof(CreateProjectCommand.Description),
                ProjectDescription.MaxLength));
    }
}