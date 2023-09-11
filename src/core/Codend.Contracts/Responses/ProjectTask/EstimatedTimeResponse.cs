namespace Codend.Contracts.Responses.ProjectTask;

public sealed record EstimatedTimeResponse
(
    int Minutes,
    int Hours,
    int Days
);