using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.Sprints.Commands.MoveTask;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Codend.Shared.Infrastructure.Lexorank;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.ProjectTaskStatuses.Commands.MoveProjectTaskStatus;

/// <summary>
/// Command used for moving sprint task to other position.
/// </summary>
/// <param name="ProjectId">Id of the project to which status belongs to.</param>
/// <param name="StatusId">If of the status that will be moved.</param>
/// <param name="Prev">Lexorank position after which status will be inserted.</param>
/// <param name="Next">Lexorank position before which status will be inserted.</param>
public record MoveProjectTaskStatusCommand(
    ProjectId ProjectId,
    ProjectTaskStatusId StatusId,
    string? Prev,
    string? Next) : ICommand;

/// <summary>
/// <see cref="MoveProjectTaskStatusCommand"/> handler.
/// </summary>
public class MoveProjectTaskStatusCommandHandler : ICommandHandler<MoveProjectTaskStatusCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTaskStatusRepository _statusRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="MoveSprintTaskCommandHandler"/> class.
    /// </summary>
    public MoveProjectTaskStatusCommandHandler(
        IUnitOfWork unitOfWork,
        IProjectTaskStatusRepository statusRepository)
    {
        _unitOfWork = unitOfWork;
        _statusRepository = statusRepository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(MoveProjectTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var status = await _statusRepository.GetByIdAsync(request.StatusId, cancellationToken);

        if (status is null)
        {
            return DomainNotFound.Fail<ProjectTaskStatus>();
        }

        if (status.ProjectId != request.ProjectId)
        {
            return Result.Fail(new DomainErrors.ProjectTaskStatus.InvalidStatusId());
        }

        var prev = request.Prev is not null ? new Lexorank(request.Prev) : null;
        var next = request.Next is not null ? new Lexorank(request.Next) : null;
        var midPosition = Lexorank.GetMiddle(prev, next);
        status.EditPosition(midPosition);

        _statusRepository.Update(status);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}