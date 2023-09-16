namespace Codend.Contracts.Requests.Epic;

public record CreateEpicRequest
(
    string Name,
    string Description,
    Guid ProjectId
);