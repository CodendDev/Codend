namespace Codend.Contracts.Responses.Project;

public sealed record ProjectResponse(
    Guid Id,
    string Name,
    string? Description,
    Guid OwnerId);