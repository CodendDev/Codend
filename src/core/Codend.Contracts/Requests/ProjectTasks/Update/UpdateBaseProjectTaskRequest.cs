using Codend.Contracts.Requests.ProjectTasks.Update.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Contracts.Requests.ProjectTasks.Update;

/// <summary>
/// Request for updating <see cref="BaseProjectTask"/>.
/// </summary>
public record UpdateBaseProjectTaskRequest
(
    string? Name,
    string? Priority,
    Guid? StatusId,
    ShouldUpdateBinder<string?>? Description,
    ShouldUpdateBinder<DateTime?>? DueDate,
    ShouldUpdateBinder<uint?>? StoryPoints,
    ShouldUpdateBinder<EstimatedTimeRequest?>? EstimatedTime,
    ShouldUpdateBinder<Guid?>? AssigneeId,
    ShouldUpdateBinder<Guid?>? StoryId
) : IUpdateBaseProjectTaskRequest;