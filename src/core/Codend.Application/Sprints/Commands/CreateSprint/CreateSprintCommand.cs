using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.Sprints.Commands.CreateSprint;

/// <summary>
/// Command for creating sprint.
/// </summary>
/// <param name="ProjectId">Project id.</param>
/// <param name="Name">Sprint name.</param>
/// <param name="StartDate">Sprint start date.</param>
/// <param name="EndDate">Sprint end date.</param>
/// <param name="Goal">Sprint goal.</param>
public sealed record CreateSprintCommand
(
    ProjectId ProjectId,
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    string? Goal
) : ICommand<Guid>;

/// <summary>
/// <see cref="CreateSprintCommand"/> handler.
/// </summary>
public class CreateSprintCommandHandler : ICommandHandler<CreateSprintCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISprintRepository _sprintRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSprintCommandHandler"/> class.
    /// </summary>
    public CreateSprintCommandHandler(IUnitOfWork unitOfWork, ISprintRepository sprintRepository)
    {
        _unitOfWork = unitOfWork;
        _sprintRepository = sprintRepository;
    }

    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(CreateSprintCommand request, CancellationToken cancellationToken)
    {
        var resultSprint = Sprint.Create(
            request.Name,
            request.ProjectId,
            request.StartDate.ToUniversalTime(),
            request.EndDate.ToUniversalTime(),
            request.Goal
        );

        if (resultSprint.IsFailed)
        {
            return resultSprint.ToResult();
        }

        var sprint = resultSprint.Value;
        _sprintRepository.Add(sprint);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return sprint.Id.Value;
    }
}