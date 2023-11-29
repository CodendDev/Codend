using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Codend.Shared.Infrastructure.Lexorank;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Sprints.Commands.MoveTask;

/// <summary>
/// Command used for moving sprint task to other position.
/// </summary>
/// <param name="ProjectId">Id of the project task belongs to.</param>
/// <param name="SprintId">Id of the sprint task belongs to.</param>
/// <param name="TaskId">If of the task that will be moved.</param>
/// <param name="Prev">Lexorank position after which tasks will be inserted.</param>
/// <param name="Next">Lexorank position before which tasks will be inserted.</param>
/// <param name="StatusId">New statusId for the task.</param>
/// <param name="Type">Task type.</param>
public record MoveSprintTaskCommand(
    ProjectId ProjectId,
    SprintId SprintId,
    ISprintTaskId TaskId,
    string? Prev,
    string? Next,
    ProjectTaskStatusId? StatusId,
    string Type
) : ICommand;

/// <summary>
/// <see cref="MoveSprintTaskCommand"/> handler.
/// </summary>
public class MoveSprintTaskCommandHandler : ICommandHandler<MoveSprintTaskCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISprintProjectTaskRepository _sprintProjectTaskRepository;
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IStoryRepository _storyRepository;
    private readonly IEpicRepository _epicRepository;
    private readonly IProjectTaskStatusRepository _statusRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="MoveSprintTaskCommandHandler"/> class.
    /// </summary>
    public MoveSprintTaskCommandHandler(
        IUnitOfWork unitOfWork,
        ISprintProjectTaskRepository sprintProjectTaskRepository,
        IProjectTaskRepository projectTaskRepository,
        IStoryRepository storyRepository,
        IEpicRepository epicRepository,
        IProjectTaskStatusRepository statusRepository)
    {
        _unitOfWork = unitOfWork;
        _sprintProjectTaskRepository = sprintProjectTaskRepository;
        _projectTaskRepository = projectTaskRepository;
        _storyRepository = storyRepository;
        _epicRepository = epicRepository;
        _statusRepository = statusRepository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(MoveSprintTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _sprintProjectTaskRepository.GetBySprintTaskIdAsync(request.TaskId, request.Type,
            cancellationToken);

        if (task is null)
        {
            return DomainNotFound.Fail<SprintProjectTask>();
        }

        if (task.SprintId != request.SprintId)
        {
            return Result.Fail(new DomainErrors.Sprint.TaskIsNotAssignedToSprint());
        }

        if (request.StatusId is not null)
        {
            var statusExist = await _statusRepository.StatusExistsWithStatusIdAsync(request.StatusId, request.ProjectId,
                cancellationToken);
            if (!statusExist)
            {
                return DomainNotFound.Fail<ProjectTaskStatus>();
            }

            var result = await EditSprintTaskStatus(request, cancellationToken);
            if (result.IsFailed)
            {
                return result;
            }
        }

        var prev = request.Prev is not null ? new Lexorank(request.Prev) : null;
        var next = request.Next is not null ? new Lexorank(request.Next) : null;
        var midPosition = Lexorank.GetMiddle(prev, next);
        task.EditPosition(midPosition);

        _sprintProjectTaskRepository.Update(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    private async Task<Result> EditSprintTaskStatus(MoveSprintTaskCommand request, CancellationToken cancellationToken)
    {
        switch (request.Type.ToLower())
        {
            case "task":
            case "base":
            case "bugfix":
                var task = await _projectTaskRepository.GetByIdAsync(
                    request.TaskId.Value.GuidConversion<ProjectTaskId>(), cancellationToken);
                if (task is null)
                {
                    return DomainNotFound.Fail<BaseProjectTask>();
                }

                task.EditStatus(request.StatusId!);
                _projectTaskRepository.Update(task);
                return Result.Ok();
            case "story":
                var story = await _storyRepository.GetByIdAsync(request.TaskId.Value.GuidConversion<StoryId>(),
                    cancellationToken);
                if (story is null)
                {
                    return DomainNotFound.Fail<Story>();
                }

                story.EditStatus(request.StatusId!);
                _storyRepository.Update(story);
                return Result.Ok();
            case "epic":
                var epic = await _epicRepository.GetByIdAsync(request.TaskId.Value.GuidConversion<EpicId>(),
                    cancellationToken);
                if (epic is null)
                {
                    return DomainNotFound.Fail<Epic>();
                }

                epic.EditStatus(request.StatusId!);
                _epicRepository.Update(epic);
                return Result.Ok();
            default:
                throw new ArgumentException("Unsupported task type");
        }
    }
}