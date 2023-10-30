using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors;
using Sprint = Codend.Domain.Entities.Sprint;

namespace Codend.Application.Sprints.Commands.AssignTasks;

/// <summary>
/// Command for assigning tasks to sprint.
/// </summary>
/// <param name="SprintId">Sprint id.</param>
/// <param name="TasksIds">List of tasks ids.</param>
public sealed record SprintAssignTasksCommand
(
    SprintId SprintId,
    IEnumerable<ISprintTaskId> TasksIds
) : ICommand;

/// <summary>
/// <see cref="SprintAssignTasksCommand"/> handler.
/// </summary>
public class SprintAssignTasksCommandHandler : ICommandHandler<SprintAssignTasksCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISprintRepository _sprintRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ISprintProjectTaskRepository _sprintProjectTaskRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SprintAssignTasksCommandHandler"/> class.
    /// </summary>
    public SprintAssignTasksCommandHandler(IUnitOfWork unitOfWork, ISprintRepository sprintRepository,
        IProjectRepository projectRepository, ISprintProjectTaskRepository sprintProjectTaskRepository)
    {
        _unitOfWork = unitOfWork;
        _sprintRepository = sprintRepository;
        _projectRepository = projectRepository;
        _sprintProjectTaskRepository = sprintProjectTaskRepository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(SprintAssignTasksCommand request, CancellationToken cancellationToken)
    {
       var sprint = await _sprintRepository.GetByIdAsync(request.SprintId, cancellationToken);
        if (sprint is null)
        {
            return General.DomainNotFound.Fail<Sprint>();
        }

        // var projectId = sprint.ProjectId;
        // var validTasks = await _projectRepository.TasksInProjectAsync(projectId, request.TasksIds, cancellationToken);
        // if (!validTasks)
        // {
        // return Result.Fail(new DomainErrors.Sprint.TaskDoesntExistInProject());
        // }

        // var sprintTasks = sprint.AssignTasks(request.TasksIds);
        // if (sprintTasks.IsFailed)
        // {
        // return sprintTasks.ToResult();
        // }

        // await _sprintProjectTaskRepository.AddRangeAsync(sprintTasks.Value, cancellationToken);
        // await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}