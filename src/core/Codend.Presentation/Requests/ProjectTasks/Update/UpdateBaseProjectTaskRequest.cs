using Codend.Application.Core;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask;
using Codend.Contracts.Requests;
using Codend.Contracts.Requests.ProjectTasks.Update;
using Codend.Domain.Entities;
using Codend.Presentation.Requests.ProjectTasks.Update.Abstractions;

namespace Codend.Presentation.Requests.ProjectTasks.Update;

/// <summary>
/// Request for updating <see cref="BaseProjectTask"/>.
/// </summary>
public record UpdateBaseProjectTaskRequest
(
    ShouldUpdateBinder<string>? _Name,
    ShouldUpdateBinder<string>? _Priority,
    ShouldUpdateBinder<string?>? _Description,
    ShouldUpdateBinder<DateTime?>? _DueDate,
    ShouldUpdateBinder<uint?>? _StoryPoints,
    ShouldUpdateBinder<Guid>? _StatusId,
    ShouldUpdateBinder<EstimatedTimeRequest>? _EstimatedTime,
    ShouldUpdateBinder<Guid?>? _AssigneeId,
    ShouldUpdateBinder<Guid?>? _StoryId
) : UpdateProjectTaskAbstractRequest<UpdateBaseProjectTaskCommand>
(
    _Name,
    _Priority,
    _Description,
    _DueDate,
    _StoryPoints,
    _StatusId,
    _EstimatedTime,
    _AssigneeId,
    _StoryId
), IUpdateBaseProjectTaskRequest
{
    /// <inheritdoc />
    public override UpdateBaseProjectTaskCommand MapToCommand()
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

        var storyId = StoryId?.Value is null
            ? ShouldUpdateProperty.DontUpdate<StoryId?>()
            : ShouldUpdateProperty.Update<StoryId?>(new StoryId(StoryId.Value.Value));

        var command = new UpdateBaseProjectTaskCommand(
            name,
            priority,
            statusId,
            description,
            estimatedTime,
            dueDate,
            storyPoints,
            assigneeId,
            storyId
        );

        return command;
    }
}