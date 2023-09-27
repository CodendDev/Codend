using Codend.Application.Core.Errors;
using Codend.Application.Extensions;
using FluentValidation;

namespace Codend.Application.Projects.Queries.GetMembers;

/// <summary>
/// Validator for <see cref="GetMembersQuery"/>.
/// </summary>
public class GetMembersQueryValidator
    : AbstractValidator<GetMembersQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetMembersQueryValidator"/> class.
    /// </summary>
    public GetMembersQueryValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new ValidationErrors.Common.PropertyNullOrEmpty(nameof(GetMembersQuery.ProjectId)));
    }
}