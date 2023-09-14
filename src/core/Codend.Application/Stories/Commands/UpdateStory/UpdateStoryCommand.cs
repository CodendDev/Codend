using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
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
    string? Description
) : ICommand;

/// <summary>
/// Handler for <see cref="UpdateStoryCommand"/>.
/// </summary>
public class UpdateStoryCommandHandler : ICommandHandler<UpdateStoryCommand>
{
    private readonly IStoryRepository _storyRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Constructs <see cref="UpdateStoryCommandHandler"/>.
    /// </summary>
    /// <param name="storyRepository"><see cref="Story"/> repository.</param>
    /// <param name="unitOfWork">Unit of work.</param>
    public UpdateStoryCommandHandler(IStoryRepository storyRepository, IUnitOfWork unitOfWork)
    {
        _storyRepository = storyRepository;
        _unitOfWork = unitOfWork;
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

        var results = new List<Result>();
        if (request.Name is not null)
        {
            results.Add(story.EditName(request.Name).ToResult());
        }

        if (request.Description is not null)
        {
            results.Add(story.EditDescription(request.Description).ToResult());
        }

        var result = Result.Merge(results.ToArray());
        if (result.IsFailed)
        {
            return result;
        }

        _storyRepository.Update(story);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}