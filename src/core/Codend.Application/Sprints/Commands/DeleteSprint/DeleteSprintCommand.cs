using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Sprints.Commands.DeleteSprint;

/// <summary>
/// Command used for deleting sprint.
/// </summary>
/// <param name="SprintId">Id of the sprint which will be deleted.</param>
public record DeleteSprintCommand(SprintId SprintId) : ICommand;

/// <summary>
/// <see cref="DeleteSprintCommand"/> handler.
/// </summary>
public class DeleteSprintCommandHandler : ICommandHandler<DeleteSprintCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISprintRepository _sprintRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSprintCommandHandler"/> class.
    /// </summary>
    public DeleteSprintCommandHandler(IUnitOfWork unitOfWork, ISprintRepository sprintRepository)
    {
        _unitOfWork = unitOfWork;
        _sprintRepository = sprintRepository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(DeleteSprintCommand request, CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetByIdAsync(request.SprintId, cancellationToken);
        if (sprint is null)
        {
            return DomainNotFound.Fail<Sprint>();
        }

        _sprintRepository.Remove(sprint);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}