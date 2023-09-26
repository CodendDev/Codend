namespace Codend.Contracts.Requests.Project;

/// <summary>
/// Represents get projects request.
/// </summary>
/// <param name="PageIndex">Index of the page that will be returned. Default 1.</param>
/// <param name="PageSize">Number of entries on one page. Default 10.</param>
/// <param name="Search">Text that will be searched in project name.</param>
/// <param name="SortColumn">Column name to sort by. Default Id.</param>
/// <param name="SortOrder">Sorting order. Ascending - 'asc', descending - 'desc'. Default 'asc'.</param>
public sealed record GetProjectsRequest(
    int PageIndex = 1,
    int PageSize = 10,
    string? Search = null,
    string? SortColumn = null,
    string? SortOrder = null);