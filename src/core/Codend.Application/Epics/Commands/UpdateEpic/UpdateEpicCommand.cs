using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;
using static Codend.Domain.Core.Errors.DomainErrors.ProjectTaskStatus;

namespace Codend.Application.Epics.Commands.UpdateEpic;

/// <summary>
/// Command used for updating an epic.
/// </summary>
/// <param name="EpicId">Id of epic which will be updated.</param>
/// <param name="Name">New name of the epic.</param>
/// <param name="Description">New description of the epic.</param>
/// <param name="StatusId">Id of the new epic status.</param>
public sealed record UpdateEpicCommand
(
    Guid EpicId,
    string? Name,
    string? Description,
    Guid? StatusId
) : ICommand;

/// <summary>
/// <see cref="UpdateEpicCommand"/> handler.
/// </summary>
public class UpdateEpicCommandHandler : ICommandHandler<UpdateEpicCommand>
{
    private readonly IEpicRepository _epicRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTaskStatusRepository _statusRepository;

    /// <summary>
    /// Constructs <see cref="UpdateEpicCommandHandler"/>.
    /// </summary>
    public UpdateEpicCommandHandler(
        IEpicRepository epicRepository,
        IUnitOfWork unitOfWork,
        IProjectTaskStatusRepository statusRepository)
    {
        _epicRepository = epicRepository;
        _unitOfWork = unitOfWork;
        _statusRepository = statusRepository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateEpicCommand request, CancellationToken cancellationToken)
    {
        var epic = await _epicRepository.GetByIdAsync(new EpicId(request.EpicId));
        var statusId = request.StatusId.GuidConversion<ProjectTaskStatusId>();

        if (epic is null)
        {
            return Result.Fail(new DomainNotFound(nameof(Epic)));
        }

        if (statusId is not null &&
            await _statusRepository.ExistsWithIdAsync(statusId, epic.ProjectId, cancellationToken) is false)
        {
            return Result.Fail(new InvalidStatusId());
        }
        
        var result = Result.Merge
        (
            request.Name.GetResultFromDelegate(epic.EditName, Result.Ok),
            request.Description.GetResultFromDelegate(epic.EditDescription, Result.Ok),
            statusId.GetResultFromDelegate(epic.EditStatus, Result.Ok)
        );

        if (result.IsFailed)
        {
            return result;
        }

        _epicRepository.Update(epic);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}