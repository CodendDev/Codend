using Codend.Application.Core.Abstractions.Querying;
using Codend.Contracts.Common;
using Codend.Contracts.Responses.Project;
using Codend.Domain.Entities;

namespace Codend.Application.Projects.Queries.GetProjects;

/// <summary>
/// Validator for GetProjectQuery.
/// </summary>
public class GetProjectsQueryValidator : AbstractQueryValidator<GetProjectsQuery, PagedList<ProjectResponse>,
    ProjectSortColumnSelector, Project>
{
}