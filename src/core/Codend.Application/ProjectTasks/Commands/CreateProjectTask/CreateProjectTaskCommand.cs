using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask;

/// <summary>
/// Create ProjectTask Command.
/// </summary>
/// <typeparam name="TProjectTaskProperties">
/// Properties interface needed for task creation.
/// Must implement <see cref="ProjectTaskProperties"/> interface.
/// </typeparam>
public interface ICreateProjectTaskCommand<out TProjectTaskProperties>
    where TProjectTaskProperties : ProjectTaskProperties
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
/// Must implement <see cref="ProjectTaskProperties"/> interface.
/// </typeparam>
public abstract class CreateProjectTaskCommandHandler<TCommand, TProjectTask, TProjectTaskProperties>
    : ICommandHandler<TCommand, Guid>
    where TCommand : ICommand<Guid>, ICreateProjectTaskCommand<TProjectTaskProperties>
    where TProjectTask : ProjectTask, IProjectTaskCreator<TProjectTask, TProjectTaskProperties>
    where TProjectTaskProperties : ProjectTaskProperties
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdentityProvider _identityProvider;

    protected CreateProjectTaskCommandHandler(
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
        request.TaskProperties.OwnerId = _identityProvider.UserId;

        var projectStatusIsValid =
            _projectTaskRepository.ProjectTaskIsValid(
                request.TaskProperties.ProjectId,
                request.TaskProperties.StatusId);
        var resultProjectTaskStatus =
            projectStatusIsValid ? Result.Ok() : Result.Fail(new DomainErrors.ProjectTaskErrors.InvalidStatusId());

        var resultTask = TProjectTask.Create(request.TaskProperties);
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