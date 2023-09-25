using System.Linq.Expressions;

namespace Codend.Application.Core.Abstractions.Querying;

/// <summary>
/// SortColumnSelector Interface.
/// </summary>
public interface ISortColumnSelector<T> where T: class
{
    /// <summary>
    /// Returns list of supported selectors as strings.
    /// </summary>
    public static abstract IReadOnlyList<string> SupportedSelectors { get; }
    
    /// <summary>
    /// Select <typeparamref name="T"/> parameter to sort by based on given string.
    /// </summary>
    /// <param name="columnName">Sort column string.</param>
    /// <typeparam name="T">Type of objects to be sorted.</typeparam>
    /// <returns></returns>
    public static abstract Expression<Func<T, object>> SortColumnSelector(string? columnName);
}