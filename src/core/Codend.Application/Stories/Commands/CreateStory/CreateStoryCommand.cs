using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.Sprints.Commands.AssignTasks;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using MediatR;
using static Codend.Domain.Core.Errors.DomainErrors.General;
using static Codend.Domain.Core.Errors.DomainErrors.ProjectTaskStatus;
using static Codend.Domain.Core.Errors.DomainErrors.StoryErrors;

namespace Codend.Application.Stories.Commands.CreateStory;

/// <summary>
/// Command user for creating user story.
/// </summary>
/// <param name="ProjectId">Story projectId.</param>
/// <param name="Name">Story name.</param>
/// <param name="Description">Story description.</param>
/// <param name="EpicId">Story epicId.</param>
/// <param name="StatusId">Story statusId.</param>
/// <param name="SprintId">Id of the sprint to which story will be assigned.</param>
public sealed record CreateStoryCommand
(
    ProjectId ProjectId,
    string Name,
    string Description,
    EpicId? EpicId,
    ProjectTaskStatusId? StatusId,
    SprintId? SprintId
) : ICommand<Guid>;

/// <summary>
/// <see cref="CreateStoryCommand"/> handler.
/// </summary>
public class CreateStoryCommandHandler : ICommandHandler<CreateStoryCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStoryRepository _storyRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTaskStatusRepository _statusRepository;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateStoryCommandHandler"/> class.
    /// </summary>
    public CreateStoryCommandHandler(
        IUnitOfWork unitOfWork,
        IStoryRepository storyRepository,
        IProjectRepository projectRepository,
        IProjectTaskStatusRepository statusRepository,
        IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _storyRepository = storyRepository;
        _projectRepository = projectRepository;
        _statusRepository = statusRepository;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
    {
        if (request.EpicId is not null
            && await _projectRepository.ProjectContainsEpic(request.ProjectId, request.EpicId) is false)
        {
            return Result.Fail(new InvalidEpicId());
        }

        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project is null)
        {
            return DomainNotFound.Fail<Project>();
        }

        if (request.StatusId is not null &&
            await _statusRepository.StatusExistsWithStatusIdAsync(request.StatusId, request.ProjectId,
                cancellationToken) is false)
        {
            return Result.Fail(new InvalidStatusId());
        }

        var storyResult = Story.Create(
            request.Name,
            request.Description,
            request.ProjectId,
            request.EpicId,
            request.StatusId ?? project.DefaultStatusId);
        if (storyResult.IsFailed)
        {
            return storyResult.ToResult();
        }


        var story = storyResult.Value;
        _storyRepository.Add(story);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Assign story to sprint if sprint id provided.
        if (request.SprintId is null) return Result.Ok(story.Id.Value);
        var result = await _mediator.Send(new SprintAssignTasksCommand(request.SprintId, new[] { story.Id }),
            cancellationToken);

        return result.IsFailed ? result : Result.Ok(story.Id.Value);
    }
}