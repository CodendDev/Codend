namespace Codend.Contracts.ProjectTasks;

public record EstimatedTimeRequest(
    uint Minutes,
    uint Hours,
    uint Days
);