using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Primitives;
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
/// <param name="Name">Story name.</param>
/// <param name="Description">Story description.</param>
/// <param name="ProjectId">Story projectId.</param>
/// <param name="EpicId">Story epicId.</param>
/// <param name="StatusId">Story statusId.</param>
public sealed record CreateStoryCommand
(
    string Name,
    string Description,
    Guid ProjectId,
    Guid? EpicId,
    Guid? StatusId
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
        var epicId = request.EpicId.GuidConversion<EpicId>();
        var projectId = request.ProjectId.GuidConversion<ProjectId>();
        var statusId = request.StatusId.GuidConversion<ProjectTaskStatusId>();

        if (epicId is not null && await _projectRepository.ProjectContainsEpic(projectId, epicId) is false)
        {
            return Result.Fail(new InvalidEpicId());
        }

        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project is null)
        {
            return DomainNotFound.Fail<Project>();
        }

        if (statusId is not null && 
            await _statusRepository.StatusExistsWithIdAsync(statusId, projectId, cancellationToken) is false)
        {
            return Result.Fail(new InvalidStatusId());
        }
        
        var storyResult = Story.Create(
            request.Name,
            request.Description,
            projectId, epicId,
            statusId ?? project.DefaultStatusId);
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