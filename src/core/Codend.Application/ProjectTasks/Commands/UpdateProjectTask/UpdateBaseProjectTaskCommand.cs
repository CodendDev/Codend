using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Contracts.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

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

public class UpdateAbstractProjectTaskCommandHandler :
    UpdateProjectTaskCommandAbstractHandler<UpdateBaseProjectTaskCommand, BaseProjectTask>
{
    public UpdateAbstractProjectTaskCommandHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
        : base(taskRepository, unitOfWork)
    {
    }
}