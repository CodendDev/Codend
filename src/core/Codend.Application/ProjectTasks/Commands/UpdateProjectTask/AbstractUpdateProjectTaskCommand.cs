using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

public interface IUpdateProjectTaskCommand
{
}

public interface IUpdateProjectTaskCommand<out TUpdateProjectTaskProperties> : IUpdateProjectTaskCommand
    where TUpdateProjectTaskProperties : UpdateProjectTaskProperties
{
    ProjectTaskId TaskId { get; }
    TUpdateProjectTaskProperties UpdateTaskProperties { get; }
}

public class AbstractUpdateProjectTaskCommandHandler<TCommand, TProjectTask, TUpdateProjectTaskProperties>
    : ICommandHandler<TCommand>
    where TCommand : ICommand, IUpdateProjectTaskCommand<TUpdateProjectTaskProperties>
    where TProjectTask : ProjectTask, IProjectTaskUpdater<TProjectTask, TUpdateProjectTaskProperties>
    where TUpdateProjectTaskProperties : UpdateProjectTaskProperties
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    protected AbstractUpdateProjectTaskCommandHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
    {
        if (await _taskRepository.GetByIdAsync(request.TaskId) is not TProjectTask task)
        {
            return Result.Fail(new DomainErrors.ProjectTaskErrors.ProjectTaskNotFound());
        }

        task.Update(request.UpdateTaskProperties);

        _taskRepository.Update(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}