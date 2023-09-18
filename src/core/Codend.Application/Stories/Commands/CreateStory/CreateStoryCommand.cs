using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;
using static Codend.Domain.Core.Errors.DomainErrors.StoryErrors;

namespace Codend.Application.Stories.Commands.CreateStory;

/// <summary>
/// Command user for creating user story.
/// </summary>
/// <param name="Name">Story name.</param>
/// <param name="Description">Story description.</param>
/// <param name="ProjectId">Story projectId.</param>
/// <param name="EpicId">Story epicId.</param>
public sealed record CreateStoryCommand
(
    string Name,
    string Description,
    Guid ProjectId,
    Guid? EpicId
) : ICommand<Guid>;

/// <summary>
/// <see cref="CreateStoryCommand"/> handler.
/// </summary>
public class CreateStoryCommandHandler : ICommandHandler<CreateStoryCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStoryRepository _storyRepository;
    private readonly IProjectRepository _projectRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateStoryCommandHandler"/> class.
    /// </summary>
    public CreateStoryCommandHandler(
        IUnitOfWork unitOfWork,
        IStoryRepository storyRepository,
        IProjectRepository projectRepository)
    {
        _unitOfWork = unitOfWork;
        _storyRepository = storyRepository;
        _projectRepository = projectRepository;
    }

    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
    {
        var epicId = request.EpicId is not null ? new EpicId(request.EpicId.Value) : null;
        var projectId = new ProjectId(request.ProjectId);

        if (epicId is not null && await _projectRepository.ProjectContainsEpic(projectId, epicId) is false)
        {
            return Result.Fail(new InvalidEpicId());
        }

        if (!await _projectRepository.Exists(projectId))
        {
            return DomainNotFound.Fail<Project>();
        }

        var storyResult = Story.Create(request.Name, request.Description, projectId, epicId);
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