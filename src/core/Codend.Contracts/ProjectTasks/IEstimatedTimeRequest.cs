namespace Codend.Contracts.ProjectTasks;

public interface IEstimatedTimeRequest
{
    uint Minutes { get; }
    uint Hours { get; }
    uint Days { get; }
}

public static class EstimatedTimeRequestExtensions
{
    public static TimeSpan? ToTimeSpan(this IEstimatedTimeRequest? request) =>
        request is not null
            ? new TimeSpan((int)request.Days, (int)request.Hours, (int)request.Minutes, 0)
            : null;
}