namespace Codend.Application.Core.Abstractions.Querying;

/// <summary>
/// Paging criteria.
/// </summary>
public interface IPageableQuery
{
    /// <summary>
    /// Page size.
    /// </summary>
    int PageSize { get; init; }

    /// <summary>
    /// Page index.
    /// </summary>
    int PageIndex { get; init; }
}