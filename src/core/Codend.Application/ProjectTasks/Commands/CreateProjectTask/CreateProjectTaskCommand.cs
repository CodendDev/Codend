using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask.Abstractions;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask;

public interface ICreateProjectTaskCommand
{
}

/// <summary>
/// Create ProjectTask Command.
/// </summary>
/// <typeparam name="TProjectTaskProperties">
/// Properties interface needed for task creation.
/// Must implement <see cref="IProjectTaskCreateProperties"/> interface.
/// </typeparam>
public interface ICreateProjectTaskCommand<out TProjectTaskProperties> : ICreateProjectTaskCommand
    where TProjectTaskProperties : IProjectTaskCreateProperties
{
    TProjectTaskProperties TaskProperties { get; }
}

/// <summary>
/// Creates ProjectTask using <see cref="IProjectTaskCreator{TProjectTask,TProps}.Create"/> and persists it.
/// </summary>
/// <typeparam name="TCommand">
/// Must implement <see cref="ICreateProjectTaskCommand{TProjectTaskProperties}"/> interface.
/// </typeparam>
/// <typeparam name="TProjectTask">
/// Must implement <see cref="ICreateProjectTaskCommand{TProjectTaskProperties}"/> interface.
/// </typeparam>
/// <typeparam name="TProjectTaskProperties">
/// Must implement <see cref="IProjectTaskCreateProperties"/> interface.
/// </typeparam>
public abstract class CreateProjectTaskCommandAbstractHandler<TCommand, TProjectTask, TProjectTaskProperties>
    : ICommandHandler<TCommand, Guid>
    where TCommand : ICommand<Guid>, ICreateProjectTaskCommand<TProjectTaskProperties>
    where TProjectTask : AbstractProjectTask, IProjectTaskCreator<TProjectTask, TProjectTaskProperties>
    where TProjectTaskProperties : IProjectTaskCreateProperties
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdentityProvider _identityProvider;

    protected CreateProjectTaskCommandAbstractHandler(
        IProjectTaskRepository projectTaskRepository,
        IUnitOfWork unitOfWork,
        IUserIdentityProvider identityProvider)
    {
        _projectTaskRepository = projectTaskRepository;
        _unitOfWork = unitOfWork;
        _identityProvider = identityProvider;
    }

    public async Task<Result<Guid>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var projectStatusIsValid =
            _projectTaskRepository.ProjectTaskIsValid(
                request.TaskProperties.ProjectId,
                request.TaskProperties.StatusId);
        var resultProjectTaskStatus =
            projectStatusIsValid ? Result.Ok() : Result.Fail(new DomainErrors.ProjectTaskErrors.InvalidStatusId());

        var resultTask = TProjectTask.Create(request.TaskProperties, _identityProvider.UserId);
        var result = Result.Merge(resultProjectTaskStatus, resultTask);
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        var task = resultTask.Value;
        _projectTaskRepository.Add(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(task.Id.Value);
    }
}