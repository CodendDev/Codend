using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Contracts.ProjectTasks;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Codend.Shared.ShouldUpdate;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

public sealed record UpdateBugfixProjectTaskCommand(
    ProjectTaskId TaskId,
    BugfixUpdateProjectTaskProperties UpdateTaskProperties
) : ICommand, IUpdateProjectTaskCommand<BugfixUpdateProjectTaskProperties>;

public class UpdateBugfixProjectTaskCommandHandler :
    AbstractUpdateProjectTaskCommandHandler<
        UpdateBugfixProjectTaskCommand,
        BugfixProjectTask,
        BugfixUpdateProjectTaskProperties>
{
    public UpdateBugfixProjectTaskCommandHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
        : base(taskRepository, unitOfWork)
    {
    }
}

public static class UpdateBugfixProjectTaskExtensions
{
    /// <summary>
    /// 💀👽
    /// </summary>
    public static UpdateBugfixProjectTaskCommand MapToCommand(this BugfixUpdateProjectTaskRequest request)
    {
        var name = request.Name ?? new ShouldUpdateProperty<string>(false);
        var priority = request.Priority ?? new ShouldUpdateProperty<string>(false);
        var statusId = request.StatusId is null
            ? new ShouldUpdateProperty<ProjectTaskStatusId> { ShouldUpdate = false }
            : new ShouldUpdateProperty<ProjectTaskStatusId>
                { ShouldUpdate = true, Value = new ProjectTaskStatusId(request.StatusId.Value) };
        var description = request.Description ?? new ShouldUpdateProperty<string?>(false);
        var estimatedTime = request.EstimatedTime is null
            ? new ShouldUpdateProperty<TimeSpan?>(false)
            : new ShouldUpdateProperty<TimeSpan?>
            {
                ShouldUpdate = true,
                Value = request.EstimatedTime.Value is null
                    ? null
                    : new TimeSpan(
                        (int)request.EstimatedTime.Value.Days,
                        (int)request.EstimatedTime.Value.Hours,
                        (int)request.EstimatedTime.Value.Minutes,
                        0)
            };
        var dueDate = request.DueDate ?? new ShouldUpdateProperty<DateTime?>(false);
        var storyPoints = request.StoryPoints ?? new ShouldUpdateProperty<uint?>(false);
        var assigneeId = request.AssigneeId is null
            ? new ShouldUpdateProperty<UserId?> { ShouldUpdate = false }
            : new ShouldUpdateProperty<UserId?>
            {
                ShouldUpdate = true,
                Value = request.AssigneeId.Value is null
                    ? null
                    : new UserId(request.AssigneeId.Value.Value) // XD
            };
        // D:

        var command = new UpdateBugfixProjectTaskCommand(
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