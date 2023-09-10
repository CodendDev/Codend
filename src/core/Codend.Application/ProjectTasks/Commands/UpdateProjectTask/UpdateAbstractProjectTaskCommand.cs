using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask.Abstractions;
using Codend.Domain.Repositories;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

public sealed record UpdateAbstractProjectTaskCommand
(
    ProjectTaskId TaskId,
    AbstractProjectTaskUpdateProperties UpdateTaskProperties
) : ICommand, IUpdateProjectTaskCommand<AbstractProjectTaskUpdateProperties>;

public class UpdateAbstractProjectTaskCommandHandler :
    AbstractUpdateProjectTaskCommandHandler<
        UpdateAbstractProjectTaskCommand,
        AbstractProjectTask,
        AbstractProjectTaskUpdateProperties>
{
    public UpdateAbstractProjectTaskCommandHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
        : base(taskRepository, unitOfWork)
    {
    }
}