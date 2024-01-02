using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Codend.Shared.Infrastructure.Lexorank;
using FluentResults;

namespace Codend.Application.ProjectTaskStatuses.Commands.CreateProjectTaskStatus;

/// <summary>
/// Command used for creating new <see cref="ProjectTaskStatus"/>.
/// </summary>
/// <param name="Name">Status name.</param>
/// <param name="ProjectId">ProjectId.</param>
public sealed record CreateProjectTaskStatusCommand
(
    string Name,
    ProjectId ProjectId
) : ICommand<Guid>;

/// <summary>
/// <see cref="CreateProjectTaskStatusCommand"/> handler.
/// </summary>
public class CreateProjectTaskStatusCommandHandler : ICommandHandler<CreateProjectTaskStatusCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTaskStatusRepository _statusRepository;
    private readonly IProjectRepository _projectRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProjectTaskStatusCommandHandler"/> class.
    /// </summary>
    public CreateProjectTaskStatusCommandHandler(
        IUnitOfWork unitOfWork,
        IProjectTaskStatusRepository statusRepository,
        IProjectRepository projectRepository)
    {
        _unitOfWork = unitOfWork;
        _statusRepository = statusRepository;
        _projectRepository = projectRepository;
    }

    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(CreateProjectTaskStatusCommand request, CancellationToken cancellationToken)
    {
        if (await _statusRepository.StatusExistsWithNameAsync(request.Name, request.ProjectId, cancellationToken))
        {
            return Result.Fail(new DomainErrors.ProjectTaskStatus.ProjectTaskStatusAlreadyExists());
        }

        var lowestStatusPosition =
            await _statusRepository.GetLowestStatusInProjectPositionAsync(request.ProjectId, cancellationToken);
        var calculatedPosition = Lexorank.GetMiddle(lowestStatusPosition);
        var status = ProjectTaskStatus.Create(request.ProjectId, request.Name, calculatedPosition);
        if (status.IsFailed)
        {
            return status.ToResult();
        }

        var taskStatus = status.Value;
        _statusRepository.Add(taskStatus);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return taskStatus.Id.Value;
    }
}