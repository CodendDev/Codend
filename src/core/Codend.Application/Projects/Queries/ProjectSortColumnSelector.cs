using System.Linq.Expressions;
using Codend.Application.Core.Abstractions.Querying;
using Codend.Domain.Entities;

namespace Codend.Application.Projects.Queries;

/// <inheritdoc/>
public abstract class ProjectSortColumnSelector : ISortColumnSelector<Project>
{
    /// <summary>
    /// Returns list of supported selectors for Project. Currently: [name].
    /// </summary>
    public static IReadOnlyList<string> SupportedSelectors => new List<string>
    {
        nameof(Project.Name).ToLower()
    };

    /// <inheritdoc />
    public static Expression<Func<Project, object>> SortColumnSelector(string? columnName) =>
        columnName?.ToLower() switch
        {
            "name" => project => project.Name.Value,
            _ => project => project.Id
        };
}