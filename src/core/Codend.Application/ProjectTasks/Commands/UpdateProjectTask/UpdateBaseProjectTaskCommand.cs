using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Contracts.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

/// <summary>
/// Record used for updating <see cref="BaseProjectTask"/> properties.
/// </summary>
public sealed record UpdateBaseProjectTaskCommand
(
    ProjectTaskId TaskId,
    IShouldUpdate<string> Name,
    IShouldUpdate<string> Priority,
    IShouldUpdate<ProjectTaskStatusId> StatusId,
    IShouldUpdate<string?> Description,
    IShouldUpdate<TimeSpan?> EstimatedTime,
    IShouldUpdate<DateTime?> DueDate,
    IShouldUpdate<uint?> StoryPoints,
    IShouldUpdate<UserId?> AssigneeId,
    IShouldUpdate<StoryId?> StoryId
) : ICommand, IUpdateProjectTaskCommand;

/// <summary>
/// Command handler for <see cref="UpdateBaseProjectTaskCommand"/>.
/// </summary>
public class UpdateAbstractProjectTaskCommandHandler :
    UpdateProjectTaskCommandAbstractHandler<UpdateBaseProjectTaskCommand, BaseProjectTask>
{
    /// <inheritdoc />
    public UpdateAbstractProjectTaskCommandHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
        : base(taskRepository, unitOfWork)
    {
    }
}