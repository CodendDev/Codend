using Codend.Contracts.ProjectTasks;

namespace Codend.Presentation.Requests;

public record EstimatedTimeRequest
(
    uint Minutes,
    uint Hours,
    uint Days
) : IEstimatedTimeRequest;