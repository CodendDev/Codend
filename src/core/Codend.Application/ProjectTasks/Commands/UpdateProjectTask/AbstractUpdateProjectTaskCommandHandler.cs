using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

/// <summary>
/// Abstract handler for updating any <see cref="AbstractProjectTask"/> that implements <see cref="IProjectTaskUpdater{TProjectTask,TUpdateProps}"/> interface
/// </summary>
/// <typeparam name="TCommand">
/// Command that implements <see cref="IUpdateProjectTaskCommand{TUpdateProjectTaskProperties}"/> interface.
/// </typeparam>
/// <typeparam name="TProjectTask">
/// ProjectTask that implements <see cref="IProjectTaskUpdater{TProjectTask,TUpdateProps}"/> interface.
/// </typeparam>
/// <typeparam name="TUpdateProjectTaskProperties">
/// ProjectTask properties which <see cref="IProjectTaskUpdater{TProjectTask,TUpdateProps}"/> uses for updating a <see cref="TProjectTask"/>.
/// </typeparam>
public abstract class AbstractUpdateProjectTaskCommandHandler<TCommand, TProjectTask, TUpdateProjectTaskProperties>
    : ICommandHandler<TCommand>
    where TCommand : ICommand, IUpdateProjectTaskCommand<TUpdateProjectTaskProperties>
    where TProjectTask : AbstractProjectTask, IProjectTaskUpdater<TProjectTask, TUpdateProjectTaskProperties>
    where TUpdateProjectTaskProperties : UpdateAbstractProjectTaskProperties
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