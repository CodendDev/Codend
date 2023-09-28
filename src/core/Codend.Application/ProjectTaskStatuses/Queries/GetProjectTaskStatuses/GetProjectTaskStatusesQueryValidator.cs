using Codend.Application.Extensions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.ProjectTaskStatuses.Queries.GetProjectTaskStatuses;

/// <summary>
/// Validates <see cref="GetProjectTaskStatusesQuery"/>.
/// </summary>
public class GetProjectTaskStatusesQueryValidator : AbstractValidator<GetProjectTaskStatusesQuery>
{
    /// <summary>
    /// Initializes validation rules for <see cref="GetProjectTaskStatusesQuery"/>.
    /// </summary>
    public GetProjectTaskStatusesQueryValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(GetProjectTaskStatusesQuery.ProjectId)));
    }
}