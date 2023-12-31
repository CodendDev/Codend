using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.Sprints.Commands.AssignTasks;
using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask.Abstractions;
using Codend.Domain.Repositories;
using FluentResults;
using MediatR;
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
    private readonly IHttpContextProvider _contextProvider;
    private readonly IStoryRepository _storyRepository;
    private readonly IProjectTaskStatusRepository _statusRepository;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProjectTaskCommandAbstractHandler{TCommand,TProjectTask,TProjectTaskProperties}"/> class.
    /// </summary>
    protected CreateProjectTaskCommandAbstractHandler(
        IProjectTaskRepository projectTaskRepository,
        IUnitOfWork unitOfWork,
        IHttpContextProvider contextProvider,
        IProjectMemberRepository projectMemberRepository,
        IStoryRepository storyRepository,
        IProjectTaskStatusRepository statusRepository,
        IMediator mediator)
    {
        _projectTaskRepository = projectTaskRepository;
        _unitOfWork = unitOfWork;
        _contextProvider = contextProvider;
        _projectMemberRepository = projectMemberRepository;
        _storyRepository = storyRepository;
        _statusRepository = statusRepository;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var userId = _contextProvider.UserId;
        var projectId = request.TaskProperties.ProjectId;

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

        // Assign task to sprint if sprint id provided.
        if (request.SprintId is null) return Result.Ok(task.Id.Value);
        var assignResult = await _mediator.Send(new SprintAssignTasksCommand(request.SprintId, new[] { task.Id }),
            cancellationToken);

        return assignResult.IsFailed ? assignResult : Result.Ok(task.Id.Value);
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