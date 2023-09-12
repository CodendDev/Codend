using System.Xml.Linq;
using Codend.Application.Core.Errors;
using Codend.Application.Extensions;
using Codend.Domain.ValueObjects;
using FluentValidation;

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
            .WithError(new ValidationErrors.Common.StringPropertyNullOrEmpty(nameof(CreateProjectCommand.Name)));
        RuleFor(x => x.Name)
            .MaximumLength(ProjectName.MaxLength)
            .WithError(new ValidationErrors.Common.StringPropertyTooLong(nameof(CreateProjectCommand.Name),
                ProjectName.MaxLength));
        RuleFor(x => x.Description)
            .MaximumLength(ProjectDescription.MaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithError(new ValidationErrors.Common.StringPropertyTooLong(nameof(CreateProjectCommand.Description),
                ProjectDescription.MaxLength));
    }
}