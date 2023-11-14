using Codend.Application.Core.Errors;
using Codend.Application.Extensions;
using Codend.Application.Projects.Queries.GetMembers;
using FluentValidation;

namespace Codend.Application.Sprints.Queries.GetAssignableTasks;

/// <summary>
/// Validator for <see cref="GetAssignableTasksQuery"/>.
/// </summary>
public class GetAssignableTasksQueryValidator : AbstractValidator<GetAssignableTasksQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetAssignableTasksQueryValidator"/> class.
    /// </summary>
    public GetAssignableTasksQueryValidator()
    {
        RuleFor(x => x.SprintId)
            .NotEmpty()
            .WithError(new ValidationErrors.Common.PropertyNullOrEmpty(nameof(GetMembersQuery.ProjectId)));

        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new ValidationErrors.Common.PropertyNullOrEmpty(nameof(GetMembersQuery.ProjectId)));
    }
}