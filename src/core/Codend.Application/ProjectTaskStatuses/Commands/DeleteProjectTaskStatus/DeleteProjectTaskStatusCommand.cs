using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;
using static Codend.Domain.Core.Errors.DomainErrors.Project;

namespace Codend.Application.ProjectTaskStatuses.Commands.DeleteProjectTaskStatus;

/// <summary>
/// Command used for deleting and task status.
/// </summary>
/// <param name="StatusId">Id of task status which will be deleted.</param>
public sealed record DeleteProjectTaskStatusCommand(Guid StatusId) : ICommand;

/// <summary>
/// <see cref="DeleteProjectTaskStatusCommand"/> handler.
/// </summary>
public class DeleteProjectTaskStatusCommandHandler : ICommandHandler<DeleteProjectTaskStatusCommand>
{
    private readonly IProjectTaskStatusRepository _statusRepository;
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IStoryRepository _storyRepository;
    private readonly IEpicRepository _epicRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProjectTaskStatusCommandHandler"/> class.
    /// </summary>
    public DeleteProjectTaskStatusCommandHandler(
        IProjectTaskStatusRepository statusRepository,
        IProjectTaskRepository taskRepository,
        IUnitOfWork unitOfWork,
        IStoryRepository storyRepository,
        IEpicRepository epicRepository)
    {
        _statusRepository = statusRepository;
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _storyRepository = storyRepository;
        _epicRepository = epicRepository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(DeleteProjectTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var statusId = request.StatusId.GuidConversion<ProjectTaskStatusId>();
        var status = await _statusRepository.GetByIdAsync(statusId, cancellationToken);
        if (status is null)
        {
            return DomainNotFound.Fail<ProjectTaskStatus>();
        }

        if (await _statusRepository.GetStatusesCountByProjectAsync(status.ProjectId, cancellationToken) < 2)
        {
            return Result.Fail(new ProjectHasToHaveProjectTaskStatus());
        }

        var defaultStatusId = await _statusRepository.GetProjectDefaultStatusIdAsync(status.ProjectId, cancellationToken);

        var statusTasks = await _taskRepository.GetTasksWithStatusId(statusId);
        var statusStories = await _storyRepository.GetStoriesWithStatusId(statusId);
        var statusEpics = await _epicRepository.GetEpicsWithStatusId(statusId);

        foreach (var task in statusTasks) task.EditStatus(defaultStatusId);
        foreach (var story in statusStories) story.EditStatus(defaultStatusId);
        foreach (var epic in statusEpics) epic.EditStatus(defaultStatusId);

        _taskRepository.UpdateRange(statusTasks);
        _statusRepository.Remove(status);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
    
}