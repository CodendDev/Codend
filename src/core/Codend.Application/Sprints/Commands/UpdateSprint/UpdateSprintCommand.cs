using Codend.Application.Core;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Contracts.Requests;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Codend.Domain.ValueObjects;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Sprints.Commands.UpdateSprint;

/// <summary>
/// Command for updating properties in sprint with given id.
/// </summary>
/// <param name="SprintId">Id of the sprint that will be updated.</param>
/// <param name="Name">New sprint name.</param>
/// <param name="StartDate">New sprint start date.</param>
/// <param name="EndDate">New sprint end date.</param>
/// <param name="Goal">New sprint goal.</param>
public record UpdateSprintCommand
(
    SprintId SprintId,
    string? Name,
    DateTime? StartDate,
    DateTime? EndDate,
    ShouldUpdateBinder<string?> Goal
) : ICommand;

/// <summary>
/// <see cref="UpdateSprintCommand"/> handler.
/// </summary>
public class UpdateSprintCommandHandler : ICommandHandler<UpdateSprintCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISprintRepository _sprintRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSprintCommandHandler"/> class.
    /// </summary>
    public UpdateSprintCommandHandler(IUnitOfWork unitOfWork, ISprintRepository sprintRepository)
    {
        _unitOfWork = unitOfWork;
        _sprintRepository = sprintRepository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateSprintCommand request, CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetByIdAsync(request.SprintId, cancellationToken);
        if (sprint is null)
        {
            return DomainNotFound.Fail<Sprint>();
        }

        Result<SprintPeriod> periodResult = Result.Ok();
        if (request.StartDate is not null && request.EndDate is not null)
        {
            periodResult = sprint.EditPeriod(
                request.StartDate.Value.ToUniversalTime(),
                request.EndDate.Value.ToUniversalTime()
            );
        }

        var result = Result.Merge(
            request.Name.GetResultFromDelegate(sprint.EditName, Result.Ok),
            request.Goal.HandleUpdateWithResult(sprint.EditGoal),
            periodResult
        );

        if (result.IsFailed)
        {
            return result;
        }

        _sprintRepository.Update(sprint);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}