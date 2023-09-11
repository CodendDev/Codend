namespace Codend.Contracts.ProjectTasks;

public record EstimatedTimeRequest
(
    uint Minutes,
    uint Hours,
    uint Days
);

public static class EstimatedTimeRequestExtensions
{
    public static TimeSpan? ToTimeSpan(this EstimatedTimeRequest? request) =>
        request is not null
            ? new TimeSpan((int)request.Days, (int)request.Hours, (int)request.Minutes, 0)
            : null;
}