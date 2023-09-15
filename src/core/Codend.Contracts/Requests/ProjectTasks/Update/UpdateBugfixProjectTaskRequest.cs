using Codend.Contracts.Abstractions;
using Codend.Contracts.Requests.ProjectTasks.Update.Abstractions;
using Codend.Domain.Entities.ProjectTask.Bugfix;

namespace Codend.Contracts.Requests.ProjectTasks.Update;

/// <summary>
/// Request for updating <see cref="BugfixProjectTask"/>.
/// </summary>
public record UpdateBugfixProjectTaskRequest
(
    ShouldUpdateBinder<string>? Name,
    ShouldUpdateBinder<string>? Priority,
    ShouldUpdateBinder<string?>? Description,
    ShouldUpdateBinder<DateTime?>? DueDate,
    ShouldUpdateBinder<uint?>? StoryPoints,
    ShouldUpdateBinder<Guid>? StatusId,
    ShouldUpdateBinder<EstimatedTimeRequest>? EstimatedTime,
    ShouldUpdateBinder<Guid?>? AssigneeId,
    ShouldUpdateBinder<Guid?>? StoryId
) : IUpdateBugfixProjectTaskRequest;