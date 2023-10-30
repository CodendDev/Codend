using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Errors;
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

    private async Task<bool> ValidateTasks<TEntity, TKey>(
        SprintAssignTasksCommand request,
        ProjectId projectId,
        CancellationToken cancellationToken)
        where TEntity : class, IProjectOwnedEntity, IEntity<TKey>
        where TKey : ISprintTaskId
    {
        var tasks = request.TasksIds.Where(t => t is TKey).ToList();
        var count = await _projectRepository.CountSprintTasksInProjectAsync<TEntity, TKey>(
            projectId,
            tasks,
            cancellationToken);
        return tasks.Count == count;
    }

    private async Task<Result> ValidateTasks(
        SprintAssignTasksCommand request,
        ProjectId projectId,
        CancellationToken cancellationToken)
    {
        if (!await ValidateTasks<BaseProjectTask, ProjectTaskId>(request, projectId, cancellationToken))
        {
            return Result.Fail(new DomainErrors.Sprint.TaskDoesntExistInProject());
        }

        if (!await ValidateTasks<Story, StoryId>(request, projectId, cancellationToken))
        {
            return Result.Fail(new DomainErrors.Sprint.TaskDoesntExistInProject());
        }

        if (!await ValidateTasks<Epic, EpicId>(request, projectId, cancellationToken))
        {
            return Result.Fail(new DomainErrors.Sprint.TaskDoesntExistInProject());
        }

        return Result.Ok();
    }

    /// <inheritdoc />
    public async Task<Result> Handle(SprintAssignTasksCommand request, CancellationToken cancellationToken)
    {
       var sprint = await _sprintRepository.GetByIdAsync(request.SprintId, cancellationToken);
        if (sprint is null)
        {
            return General.DomainNotFound.Fail<Sprint>();
        }

        var projectId = sprint.ProjectId;

        // validate if tasks are owned by project
        var validTasks = await ValidateTasks(request, projectId, cancellationToken);
        if (validTasks.IsFailed)
        {
            return validTasks;
        }

        // validate if task already exist
        if (await _sprintRepository.SprintTasksExistsInSprintAsync(request.SprintId, request.TasksIds,
                cancellationToken))
        {
            return Result.Fail(new DomainErrors.Sprint.TaskIsAlreadyAssignedToSprint());
        }

        // assign tasks to sprint
        var sprintTasks = sprint.AssignTasks(request.TasksIds);
        if (sprintTasks.IsFailed)
        {
            return sprintTasks.ToResult();
        }

        await _sprintProjectTaskRepository.AddRangeAsync(sprintTasks.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}