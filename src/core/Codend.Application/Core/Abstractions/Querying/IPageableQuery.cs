namespace Codend.Application.Core.Abstractions.Querying;

/// <summary>
/// Paging criteria.
/// </summary>
public interface IPageableQuery
{
    /// <summary>
    /// Minimum page size.
    /// </summary>
    public const int MinPageSize = 1;
    /// <summary>
    /// Maximum page size.
    /// </summary>
    public const int MaxPageSize = 100;
    /// <summary>
    /// Minimum page index.
    /// </summary>
    public const int MinPageIndex = 1;
    
    /// <summary>
    /// Page size.
    /// </summary>
    int PageSize { get; init; }

    /// <summary>
    /// Page index.
    /// </summary>
    int PageIndex { get; init; }
}