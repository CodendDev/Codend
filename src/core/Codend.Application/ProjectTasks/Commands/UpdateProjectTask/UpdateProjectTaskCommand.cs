using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

public sealed record UpdateProjectTaskCommand
(
    ProjectTaskId TaskId,
    UpdateProjectTaskProperties UpdateTaskProperties
) : ICommand, IUpdateProjectTaskCommand<UpdateProjectTaskProperties>;

public class UpdateProjectTaskCommandHandler :
    AbstractUpdateProjectTaskCommandHandler<
        UpdateProjectTaskCommand,
        AbstractProjectTask,
        UpdateProjectTaskProperties>
{
    public UpdateProjectTaskCommandHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
        : base(taskRepository, unitOfWork)
    {
    }
}