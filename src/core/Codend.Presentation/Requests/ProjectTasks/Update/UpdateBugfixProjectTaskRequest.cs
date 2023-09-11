using Codend.Application.Core;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask;
using Codend.Contracts.ProjectTasks;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask.Bugfix;

namespace Codend.Presentation.Requests.ProjectTasks.Update;

/// <summary>
/// Request for updating <see cref="BugfixProjectTask"/>.
/// </summary>
public record UpdateBugfixProjectTaskRequest
(
    Guid TaskId,
    ShouldUpdateBinder<string>? _Name,
    ShouldUpdateBinder<string>? _Priority,
    ShouldUpdateBinder<string?>? _Description,
    ShouldUpdateBinder<DateTime?>? _DueDate,
    ShouldUpdateBinder<uint?>? _StoryPoints,
    ShouldUpdateBinder<Guid>? _StatusId,
    ShouldUpdateBinder<EstimatedTimeRequest>? _EstimatedTime,
    ShouldUpdateBinder<Guid?>? _AssigneeId
) : AbstractUpdateProjectTaskRequest<UpdateBugfixProjectTaskCommand>
(
    TaskId,
    _Name,
    _Priority,
    _Description,
    _DueDate,
    _StoryPoints,
    _StatusId,
    _EstimatedTime,
    _AssigneeId
), IUpdateBugfixProjectTaskRequest
{
    /// <inheritdoc />
    public override UpdateBugfixProjectTaskCommand MapToCommand()
    {
        var name = Name ?? ShouldUpdateProperty.DontUpdate<string>();
        var priority = Priority ?? ShouldUpdateProperty.DontUpdate<string>();
        var description = Description ?? ShouldUpdateProperty.DontUpdate<string?>();
        var dueDate = DueDate ?? ShouldUpdateProperty.DontUpdate<DateTime?>();
        var storyPoints = StoryPoints ?? ShouldUpdateProperty.DontUpdate<uint?>();

        var statusId = StatusId is null
            ? ShouldUpdateProperty.DontUpdate<ProjectTaskStatusId>()
            : ShouldUpdateProperty.Update(new ProjectTaskStatusId(StatusId.Value));

        var estimatedTime = EstimatedTime is null
            ? ShouldUpdateProperty.DontUpdate<TimeSpan?>()
            : ShouldUpdateProperty.Update(EstimatedTime.Value.ToTimeSpan());

        var assigneeId = AssigneeId?.Value is null
            ? ShouldUpdateProperty.DontUpdate<UserId?>()
            : ShouldUpdateProperty.Update<UserId?>(new UserId(AssigneeId.Value.Value));

        var command = new UpdateBugfixProjectTaskCommand(
            new ProjectTaskId(TaskId),
            new BugfixProjectTaskUpdateProperties(
                name,
                priority,
                statusId,
                description,
                estimatedTime,
                dueDate,
                storyPoints,
                assigneeId
            )
        );

        return command;
    }
}