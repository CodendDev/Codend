using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Stories.Commands.DeleteStory;

/// <summary>
/// Command used for deleting user story.
/// </summary>
/// <param name="StoryId">Id of story which will be deleted.</param>
public sealed record DeleteStoryCommand(Guid StoryId) : ICommand;

/// <summary>
/// <see cref="DeleteStoryCommand"/> handler.
/// </summary>
public class DeleteStoryCommandHandler : ICommandHandler<DeleteStoryCommand>
{
    private readonly IStoryRepository _storyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTaskRepository _taskRepository;

    /// <summary>
    /// Constructs <see cref="DeleteStoryCommandHandler"/> handler.
    /// </summary>
    /// <param name="storyRepository"><see cref="Story"/> repository.</param>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="taskRepository"><see cref="BaseProjectTask"/> repository.</param>
    public DeleteStoryCommandHandler(
        IStoryRepository storyRepository,
        IUnitOfWork unitOfWork,
        IProjectTaskRepository taskRepository)
    {
        _storyRepository = storyRepository;
        _unitOfWork = unitOfWork;
        _taskRepository = taskRepository;
    }

    /// <summary>
    /// Handles <see cref="DeleteStoryCommand"/> request. Deletes references of story inside <see cref="BaseProjectTask"/>s and deletes a story.
    /// </summary>
    /// <param name="request"><see cref="DeleteStoryCommand"/> request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><see cref="Result"/>.Ok() or a failure with errors.</returns>
    public async Task<Result> Handle(DeleteStoryCommand request, CancellationToken cancellationToken)
    {
        var story = await _storyRepository.GetByIdAsync(new StoryId(request.StoryId), cancellationToken);

        if (story is null)
        {
            return DomainNotFound.Fail<Story>();
        }

        var storyTasks = await _taskRepository.GetStoryTasks(story.Id, cancellationToken);
        foreach (var task in storyTasks)
        {
            task.EditStory(null);
        }

        _taskRepository.UpdateRange(storyTasks);
        _storyRepository.Remove(story);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}