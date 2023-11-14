using Codend.Application.Core.Errors;
using Codend.Application.Extensions;
using Codend.Application.Projects.Queries.GetMembers;
using FluentValidation;

namespace Codend.Application.Sprints.Queries.GetSprint;

/// <summary>
/// Validator for <see cref="GetSprintQuery"/>.
/// </summary>
public class GetSprintQueryValidator : AbstractValidator<GetSprintQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSprintQueryValidator"/> class.
    /// </summary>
    public GetSprintQueryValidator()
    {
        RuleFor(x => x.SprintId)
            .NotEmpty()
            .WithError(new ValidationErrors.Common.PropertyNullOrEmpty(nameof(GetMembersQuery.ProjectId)));

        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new ValidationErrors.Common.PropertyNullOrEmpty(nameof(GetMembersQuery.ProjectId)));
    }
}