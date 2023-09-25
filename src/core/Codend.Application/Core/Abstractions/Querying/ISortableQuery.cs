namespace Codend.Application.Core.Abstractions.Querying;

/// <summary>
/// Sorting criteria.
/// </summary>
public interface ISortableQuery
{
    /// <summary>
    /// Column to sort by.
    /// </summary>
    string? SortColumn { get; init; }
    
    /// <summary>
    /// Sorting order - 'desc' or 'asc'.
    /// </summary>
    string? SortOrder { get; init; }
}