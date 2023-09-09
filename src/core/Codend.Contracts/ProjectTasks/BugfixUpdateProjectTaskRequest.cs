using Codend.Shared.ShouldUpdate;

namespace Codend.Contracts.ProjectTasks;

public record BugfixUpdateProjectTaskRequest
(
    Guid TaskId,
    ShouldUpdateProperty<string>? Name,
    ShouldUpdateProperty<string>? Priority,
    ShouldUpdateProperty<Guid>? StatusId,
    ShouldUpdateProperty<string?>? Description,
    ShouldUpdateProperty<EstimatedTimeRequest?>? EstimatedTime,
    ShouldUpdateProperty<DateTime?>? DueDate,
    ShouldUpdateProperty<uint?>? StoryPoints,
    ShouldUpdateProperty<Guid?>? AssigneeId
) : UpdateProjectTaskRequest
(
    TaskId,
    Name,
    Priority,
    StatusId,
    Description,
    EstimatedTime,
    DueDate,
    StoryPoints,
    AssigneeId
);