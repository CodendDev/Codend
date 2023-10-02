using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Epics.Commands.DeleteEpic;

/// <summary>
/// Command used for deleting an epic.
/// </summary>
/// <param name="EpicId">Id of the epic which will be deleted.</param>
public sealed record DeleteEpicCommand(EpicId EpicId) : ICommand;

/// <summary>
/// <see cref="DeleteEpicCommand"/> handler.
/// </summary>
public class DeleteEpicCommandHandler : ICommandHandler<DeleteEpicCommand>
{
    private readonly IEpicRepository _epicRepository;
    private readonly IStoryRepository _storyRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteEpicCommandHandler"/> class.
    /// </summary>
    public DeleteEpicCommandHandler(
        IEpicRepository epicRepository,
        IStoryRepository storyRepository,
        IUnitOfWork unitOfWork)
    {
        _epicRepository = epicRepository;
        _storyRepository = storyRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles <see cref="DeleteEpicCommand"/> request. Deletes references of the epic inside <see cref="Story"/> and deletes the epic.
    /// </summary>
    /// <param name="request"><see cref="DeleteEpicCommand"/> request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><see cref="Result"/>.Ok() or a failure with errors.</returns>
    public async Task<Result> Handle(DeleteEpicCommand request, CancellationToken cancellationToken)
    {
        var epic = await _epicRepository.GetByIdAsync(request.EpicId, cancellationToken);

        if (epic is null)
        {
            return Result.Fail(new DomainNotFound(nameof(Epic)));
        }

        var epicStories = await _storyRepository.GetStoriesByEpicIdAsync(epic.Id, cancellationToken);
        foreach (var story in epicStories)
        {
            story.EditEpicId(null);
        }

        _storyRepository.UpdateRange(epicStories);
        _epicRepository.Remove(epic);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}