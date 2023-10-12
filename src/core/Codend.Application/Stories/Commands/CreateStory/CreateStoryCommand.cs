using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
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
public sealed record CreateStoryCommand
(
    ProjectId ProjectId,
    string Name,
    string Description,
    EpicId? EpicId,
    ProjectTaskStatusId? StatusId
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

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateStoryCommandHandler"/> class.
    /// </summary>
    public CreateStoryCommandHandler(
        IUnitOfWork unitOfWork,
        IStoryRepository storyRepository,
        IProjectRepository projectRepository,
        IProjectTaskStatusRepository statusRepository)
    {
        _unitOfWork = unitOfWork;
        _storyRepository = storyRepository;
        _projectRepository = projectRepository;
        _statusRepository = statusRepository;
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

        return Result.Ok(story.Id.Value);
    }
}