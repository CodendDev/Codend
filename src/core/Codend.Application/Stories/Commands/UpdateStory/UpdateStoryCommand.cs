using Codend.Application.Core;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Contracts.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StoryErrors;

namespace Codend.Application.Stories.Commands.UpdateStory;

/// <summary>
/// Command used for updating a story.
/// </summary>
/// <param name="StoryId">Id of story which will be updated.</param>
/// <param name="Name">New name of the story.</param>
/// <param name="Description">New description of the story.</param>
public sealed record UpdateStoryCommand
(
    Guid StoryId,
    string? Name,
    string? Description,
    ShouldUpdateBinder<EpicId?> EpicId
) : ICommand;

/// <summary>
/// Handler for <see cref="UpdateStoryCommand"/>.
/// </summary>
public class UpdateStoryCommandHandler : ICommandHandler<UpdateStoryCommand>
{
    private readonly IStoryRepository _storyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectRepository _projectRepository;

    /// <summary>
    /// Constructs <see cref="UpdateStoryCommandHandler"/>.
    /// </summary>
    public UpdateStoryCommandHandler(
        IStoryRepository storyRepository,
        IUnitOfWork unitOfWork,
        IProjectRepository projectRepository)
    {
        _storyRepository = storyRepository;
        _unitOfWork = unitOfWork;
        _projectRepository = projectRepository;
    }

    /// <summary>
    /// Handles <see cref="UpdateStoryCommand"/>. Updates <see cref="Story"/> name and description.
    /// </summary>
    /// <param name="request">Command request.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Result"/>.Ok() or a failure with errors.</returns>
    public async Task<Result> Handle(UpdateStoryCommand request, CancellationToken cancellationToken)
    {
        var story = await _storyRepository.GetByIdAsync(new StoryId(request.StoryId));

        if (story is null)
        {
            return Result.Fail(new StoryNotFound());
        }

        if (request.EpicId.ShouldUpdate &&
            await _projectRepository.ProjectContainsEpic(story.ProjectId, request.EpicId.Value!) is false)
        {
            return Result.Fail(new InvalidEpicId());
        }

        var result = Result.Merge(
            request.Name is null ? null : story.EditName(request.Name).ToResult(),
            request.Description is null ? null : story.EditDescription(request.Description).ToResult(),
            request.EpicId.HandleUpdate(story.EditEpicId)
        );

        if (result.IsFailed)
        {
            return result;
        }

        _storyRepository.Update(story);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}