using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Codend.Shared;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

public sealed record UpdateAbstractProjectTaskCommand
(
    ProjectTaskId TaskId,
    IShouldUpdate<string> Name,
    IShouldUpdate<string> Priority,
    IShouldUpdate<ProjectTaskStatusId> StatusId,
    IShouldUpdate<string?> Description,
    IShouldUpdate<TimeSpan?> EstimatedTime,
    IShouldUpdate<DateTime?> DueDate,
    IShouldUpdate<uint?> StoryPoints,
    IShouldUpdate<UserId?> AssigneeId
) : ICommand, IUpdateProjectTaskCommand;

public class UpdateAbstractProjectTaskCommandHandler :
    AbstractUpdateProjectTaskCommandHandler<UpdateAbstractProjectTaskCommand, AbstractProjectTask>
{
    public UpdateAbstractProjectTaskCommandHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
        : base(taskRepository, unitOfWork)
    {
    }
}