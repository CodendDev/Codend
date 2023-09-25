using System.Linq.Expressions;
using Codend.Application.Core.Abstractions.Querying;
using Codend.Domain.Entities;

namespace Codend.Application.Projects.Queries;

/// <inheritdoc/>
internal abstract class ProjectSortColumnSelector : ISortColumnSelector<Project>
{
    /// <inheritdoc />
    public static Expression<Func<Project, object>> SortColumnSelector(string? columnName)=>
        columnName?.ToLower() switch
        {
            "name" => project => project.Name,
            _ => project => project.Id
        };
}