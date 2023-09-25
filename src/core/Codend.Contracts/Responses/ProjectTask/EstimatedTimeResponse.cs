namespace Codend.Contracts.Responses.ProjectTask;

/// <summary>
/// Represents EstimatedTime response
/// </summary>
public sealed record EstimatedTimeResponse
(
    int Minutes,
    int Hours,
    int Days
);