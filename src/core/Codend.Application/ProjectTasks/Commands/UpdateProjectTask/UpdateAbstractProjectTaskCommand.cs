using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

public sealed record UpdateAbstractProjectTaskCommand
(
    ProjectTaskId TaskId,
    UpdateAbstractProjectTaskProperties UpdateTaskProperties
) : ICommand, IUpdateProjectTaskCommand<UpdateAbstractProjectTaskProperties>;

public class UpdateAbstractProjectTaskCommandHandler :
    AbstractUpdateProjectTaskCommandHandler<
        UpdateAbstractProjectTaskCommand,
        AbstractProjectTask,
        UpdateAbstractProjectTaskProperties>
{
    public UpdateAbstractProjectTaskCommandHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
        : base(taskRepository, unitOfWork)
    {
    }
}