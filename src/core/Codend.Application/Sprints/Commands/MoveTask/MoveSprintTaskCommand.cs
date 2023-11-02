using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Codend.Shared.Infrastructure.Lexorank;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Sprints.Commands.MoveTask;

/// <summary>
/// Command used for moving sprint task to other position.
/// </summary>
/// <param name="SprintId">Id of the sprint task belongs to.</param>
/// <param name="TaskId">If of the task that will be moved.</param>
/// <param name="Prev">Lexorank position after which tasks will be inserted.</param>
/// <param name="Next">Lexorank position before which tasks will be inserted.</param>
public record MoveSprintTaskCommand(SprintId SprintId, ISprintTaskId TaskId, string? Prev, string? Next) : ICommand;

/// <summary>
/// <see cref="MoveSprintTaskCommand"/> handler.
/// </summary>
public class MoveSprintTaskCommandHandler : ICommandHandler<MoveSprintTaskCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISprintProjectTaskRepository _sprintProjectTaskRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="MoveSprintTaskCommandHandler"/> class.
    /// </summary>
    public MoveSprintTaskCommandHandler(IUnitOfWork unitOfWork,
        ISprintProjectTaskRepository sprintProjectTaskRepository)
    {
        _unitOfWork = unitOfWork;
        _sprintProjectTaskRepository = sprintProjectTaskRepository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(MoveSprintTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _sprintProjectTaskRepository.GetBySprintTaskIdAsync(request.TaskId, cancellationToken);

        if (task is null)
        {
            return DomainNotFound.Fail<SprintProjectTask>();
        }

        if (task.SprintId != request.SprintId)
        {
            return Result.Fail(new DomainErrors.Sprint.TaskIsNotAssignedToSprint());
        }

        var prev = request.Prev is not null ? new Lexorank(request.Prev) : null;
        var next = request.Next is not null ? new Lexorank(request.Next) : null;
        var midPosition = Lexorank.GetMiddle(prev, next);
        task.EditPosition(midPosition);

        _sprintProjectTaskRepository.Update(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}