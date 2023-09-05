using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask;

public interface ICreateProjectTaskCommand<out TProjectTaskProperties>
{
    TProjectTaskProperties TaskProperties { get; }
}

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

    public virtual async Task<Result<Guid>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        request.TaskProperties.OwnerId = _identityProvider.UserId;

        var result = TProjectTask.Create(request.TaskProperties);
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        var task = result.Value;
        _projectTaskRepository.Add(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(task.Id.Value);
    }
}