using Codend.Application.Core.Errors;
using Codend.Application.Extensions;
using FluentValidation;

namespace Codend.Application.ProjectTasks.Commands.AssignUser;

/// <summary>
/// Validator for <see cref="AssignUserCommand"/>
/// </summary>
public class AssignUserCommandValidator : AbstractValidator<AssignUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AssignUserCommandValidator"/> class.
    /// </summary>
    public AssignUserCommandValidator()
    {
        RuleFor(x => x.ProjectTaskId)
            .NotEmpty()
            .WithError(new ValidationErrors.Common.PropertyNullOrEmpty(nameof(AssignUserCommand.ProjectTaskId)));
        
        RuleFor(x => x.AssigneeId)
            .NotEmpty()
            .WithError(new ValidationErrors.Common.PropertyNullOrEmpty(nameof(AssignUserCommand.AssigneeId)));
    }
}