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
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProjectTaskStatusCommandHandler"/> class.
    /// </summary>
    public DeleteProjectTaskStatusCommandHandler(
        IProjectTaskStatusRepository statusRepository,
        IProjectTaskRepository taskRepository,
        IUnitOfWork unitOfWork)
    {
        _statusRepository = statusRepository;
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
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

        var defaultStatus = await _statusRepository.GetDefaultStatusInProjectAsync(status.ProjectId, cancellationToken);
        var statusTasks = _taskRepository.GetTasksByTaskStatusId(statusId).ToList();
        foreach (var task in statusTasks)
        {
            var result = task.EditStatus(defaultStatus.Id);
            if (result.IsFailed)
            {
                return result.ToResult();
            }
        }

        _taskRepository.UpdateRange(statusTasks);
        _statusRepository.Remove(status);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}