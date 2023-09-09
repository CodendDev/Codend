using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Contracts.ProjectTasks;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Codend.Shared.ShouldUpdate;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

public sealed record UpdateProjectTaskCommand
(
    ProjectTaskId TaskId,
    UpdateProjectTaskProperties UpdateTaskProperties
) : ICommand, IUpdateProjectTaskCommand<UpdateProjectTaskProperties>;

public class UpdateProjectTaskCommandHandler :
    AbstractUpdateProjectTaskCommandHandler<
        UpdateProjectTaskCommand,
        ProjectTask,
        UpdateProjectTaskProperties>
{
    public UpdateProjectTaskCommandHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
        : base(taskRepository, unitOfWork)
    {
    }
}

public static class UpdateProjectTaskExtensions
{
    /// <summary>
    /// 💀👽
    /// </summary>
    public static UpdateProjectTaskCommand MapToCommand(this UpdateProjectTaskRequest request)
    {
        var name = request.Name ?? ShouldUpdateProperty.DontUpdate<string>();
        var priority = request.Priority ?? ShouldUpdateProperty.DontUpdate<string>();
        var description = request.Description ?? ShouldUpdateProperty.DontUpdate<string?>();
        var dueDate = request.DueDate ?? ShouldUpdateProperty.DontUpdate<DateTime?>();
        var storyPoints = request.StoryPoints ?? ShouldUpdateProperty.DontUpdate<uint?>();

        var statusId = request.StatusId is null
            ? ShouldUpdateProperty.DontUpdate<ProjectTaskStatusId>()
            : ShouldUpdateProperty.Update(new ProjectTaskStatusId(request.StatusId.Value));

        var estimatedTime = request.EstimatedTime is null
            ? ShouldUpdateProperty.DontUpdate<TimeSpan?>()
            : ShouldUpdateProperty.Update(request.EstimatedTime.Value.ToTimeSpan());

        var assigneeId = request.AssigneeId?.Value is null
            ? ShouldUpdateProperty.DontUpdate<UserId?>()
            : ShouldUpdateProperty.Update<UserId?>(new UserId(request.AssigneeId.Value.Value));

        var command = new UpdateProjectTaskCommand(
            new ProjectTaskId(request.TaskId),
            new BugfixUpdateProjectTaskProperties(
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