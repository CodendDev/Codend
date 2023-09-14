using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.Stories.Commands.CreateStory;

/// <summary>
/// Command user for creating user story.
/// </summary>
/// <param name="Name">Story name.</param>
/// <param name="Description">Story description.</param>
/// <param name="ProjectId">Story projectId.</param>
public sealed record CreateStoryCommand
(
    string Name,
    string Description,
    Guid ProjectId
) : ICommand<Guid>;

public class CreateStoryCommandHandler : ICommandHandler<CreateStoryCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStoryRepository _storyRepository;

    public CreateStoryCommandHandler(IUnitOfWork unitOfWork, IStoryRepository storyRepository)
    {
        _unitOfWork = unitOfWork;
        _storyRepository = storyRepository;
    }

    public async Task<Result<Guid>> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
    {
        var storyResult = Story.Create(request.Name, request.Description, new ProjectId(request.ProjectId));
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