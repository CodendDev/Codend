using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask.Abstractions;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;
using static Codend.Domain.Core.Errors.DomainErrors.ProjectTaskErrors;
using static Codend.Domain.Core.Errors.DomainErrors.ProjectTaskStatus;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;

/// <summary>
/// Abstract handler for commands implementing <see cref="ICreateProjectTaskCommand{TProjectTaskProperties}"/> interface.
/// Creates <typeparamref name="TProjectTask"/> using <see cref="IProjectTaskCreator{TProjectTask,TProps}.Create"/> and persists it.
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
public class CreateProjectTaskCommandAbstractHandler<TCommand, TProjectTask, TProjectTaskProperties>
    : ICommandHandler<TCommand, Guid>
    where TCommand : ICommand<Guid>, ICreateProjectTaskCommand<TProjectTaskProperties>
    where TProjectTask : BaseProjectTask, IProjectTaskCreator<TProjectTask, TProjectTaskProperties>
    where TProjectTaskProperties : class, IProjectTaskCreateProperties
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdentityProvider _identityProvider;
    private readonly IStoryRepository _storyRepository;
    private readonly IProjectTaskStatusRepository _statusRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProjectTaskCommandAbstractHandler{TCommand,TProjectTask,TProjectTaskProperties}"/> class.
    /// </summary>
    protected CreateProjectTaskCommandAbstractHandler(
        IProjectTaskRepository projectTaskRepository,
        IUnitOfWork unitOfWork,
        IUserIdentityProvider identityProvider,
        IProjectMemberRepository projectMemberRepository,
        IStoryRepository storyRepository,
        IProjectTaskStatusRepository statusRepository)
    {
        _projectTaskRepository = projectTaskRepository;
        _unitOfWork = unitOfWork;
        _identityProvider = identityProvider;
        _projectMemberRepository = projectMemberRepository;
        _storyRepository = storyRepository;
        _statusRepository = statusRepository;
    }

    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        // Validate current user and it's permissions.
        var userId = _identityProvider.UserId;
        var projectId = request.TaskProperties.ProjectId;
        if (!await _projectMemberRepository
                .IsProjectMember(userId, projectId, cancellationToken))
        {
            return DomainNotFound.Fail<Project>();
        }

        var resultProjectTaskStatus = await ValidateStatus(request, cancellationToken);

        // Validate assignee id.
        var assigneeId = request.TaskProperties.AssigneeId;
        if (assigneeId is not null &&
            !await _projectMemberRepository.IsProjectMember(assigneeId, projectId, cancellationToken))
        {
            return Result.Fail(new InvalidAssigneeId());
        }

        // Validate story id.
        var storyId = request.TaskProperties.StoryId;
        if (storyId is not null)
        {
            var story = await _storyRepository.GetByIdAsync(storyId, cancellationToken);
            if (story is null || story.ProjectId != projectId)
                return Result.Fail(new InvalidStoryId());
        }

        var resultTask = TProjectTask.Create(request.TaskProperties, userId);
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

    // Validate status id, if null set defaultStatusId as statusId.
    private async Task<Result> ValidateStatus(TCommand request, CancellationToken cancellationToken)
    {
        var statusId = request.TaskProperties.StatusId;
        var projectId = request.TaskProperties.ProjectId;

        if (statusId is not null)
        {
            var projectStatusIsValid = _projectTaskRepository.ProjectTaskStatusIsValid(projectId, statusId);
            return projectStatusIsValid ? Result.Ok() : Result.Fail(new InvalidStatusId());
        }

        var defaultStatusId = await _statusRepository.GetProjectDefaultStatusIdAsync(projectId, cancellationToken);
        request.TaskProperties.StatusId = defaultStatusId;
        return Result.Ok();
    }
}