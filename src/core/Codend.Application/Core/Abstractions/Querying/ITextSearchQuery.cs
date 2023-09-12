namespace Codend.Application.Core.Abstractions.Querying;

/// <summary>
/// Searching critera.
/// </summary>
public interface ITextSearchQuery
{
    /// <summary>
    /// Text to search for.
    /// </summary>
    string? Search { get; set; }
}