using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors;
using static Codend.Domain.Core.Errors.DomainErrors.Sprint;
using Sprint = Codend.Domain.Entities.Sprint;

namespace Codend.Application.Sprints.Commands.RemoveTasks;

/// <summary>
/// Command for removing tasks to sprint.
/// </summary>
/// <param name="SprintId">Sprint id.</param>
/// <param name="TasksIds">List of tasks ids.</param>
public sealed record SprintRemoveTasksCommand
(
    SprintId SprintId,
    IEnumerable<ProjectTaskId> TasksIds
) : ICommand;

/// <summary>
/// <see cref="SprintRemoveTasksCommand"/> handler.
/// </summary>
public class SprintRemoveTasksCommandHandler : ICommandHandler<SprintRemoveTasksCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISprintRepository _sprintRepository;
    private readonly ISprintProjectTaskRepository _sprintProjectTaskRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SprintRemoveTasksCommandHandler"/> class.
    /// </summary>
    public SprintRemoveTasksCommandHandler(
        IUnitOfWork unitOfWork,
        ISprintRepository sprintRepository,
        ISprintProjectTaskRepository sprintProjectTaskRepository)
    {
        _unitOfWork = unitOfWork;
        _sprintRepository = sprintRepository;
        _sprintProjectTaskRepository = sprintProjectTaskRepository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(SprintRemoveTasksCommand request, CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetByIdAsync(request.SprintId, cancellationToken);
        if (sprint is null)
        {
            return General.DomainNotFound.Fail<Sprint>();
        }

        var sprintProjectTasks = await _sprintProjectTaskRepository
            .GetRangeBySprintIdAndProjectTaskIds(request.SprintId, request.TasksIds);

        if (sprintProjectTasks.Count != request.TasksIds.Count())
        {
            return Result.Fail(new TaskIsNotAssignedToSprint());
        }

        _sprintProjectTaskRepository.RemoveRange(sprintProjectTasks);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}