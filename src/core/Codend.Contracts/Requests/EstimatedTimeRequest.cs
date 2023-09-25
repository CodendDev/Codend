namespace Codend.Contracts.Requests;

/// <summary>
/// Represents EstimatedTime request. 
/// </summary>
public record EstimatedTimeRequest
(
    uint Minutes,
    uint Hours,
    uint Days
);

/// <summary>
/// <see cref="EstimatedTimeRequest"/> extensions.
/// </summary>
public static class EstimatedTimeRequestExtensions
{
    /// <summary>
    /// Converts <see cref="EstimatedTimeRequest"/> object to <see cref="TimeSpan"/> object.
    /// If <see cref="EstimatedTimeRequest"/> is null returns null.
    /// </summary>
    /// <param name="request">Request to be converted.</param>
    /// <returns>Timespan or null.</returns>
    public static TimeSpan? ToTimeSpan(this EstimatedTimeRequest? request) =>
        request is not null
            ? new TimeSpan((int)request.Days, (int)request.Hours, (int)request.Minutes, 0)
            : null;
}