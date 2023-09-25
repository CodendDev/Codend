using System.Linq.Expressions;
using Codend.Application.Core.Abstractions.Querying;

namespace Codend.Application.Extensions;

/// <summary>
/// IQueryable extensions class.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Pagination extension method.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    /// <param name="query">Query.</param>
    /// <param name="pageableQuery">Paging criteria.</param>
    /// <returns>Paginated query.</returns>
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, IPageableQuery pageableQuery)
    {
        var result = query.Skip(pageableQuery.PageSize * (pageableQuery.PageIndex - 1)).Take(pageableQuery.PageSize);
        return result;
    }

    /// <summary>
    /// Sorting extension method.
    /// </summary>
    /// <param name="query">Query.</param>
    /// <param name="sortableQuery">Sorting criteria.</param>
    /// <param name="sortColumnSelector">Sorting column selector expression.</param>
    /// <typeparam name="T">Entity type.</typeparam>
    /// <returns></returns>
    public static IQueryable<T> Sort<T>(this IQueryable<T> query, ISortableQuery sortableQuery, Expression<Func<T, object>> sortColumnSelector)
    {
        return sortableQuery.SortOrder?.ToLower() == "desc" ? query.OrderByDescending(sortColumnSelector) : query.OrderBy(sortColumnSelector);
    }
}