using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.ProjectTaskStatuses.Commands.UpdateProjectTaskStatus;

/// <summary>
/// Command used for updating ProjectTaskStatus
/// </summary>
/// <param name="StatusId">Id of task status which will be updated.</param>
/// <param name="Name">New name of task status.</param>
public sealed record UpdateProjectTaskStatusCommand
(
    Guid StatusId,
    string Name
) : ICommand;

/// <summary>
/// Handler for <see cref="UpdateProjectTaskStatusCommand"/>.
/// </summary>
public class UpdateProjectTaskStatusCommandHandler : ICommandHandler<UpdateProjectTaskStatusCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTaskStatusRepository _statusRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProjectTaskStatusCommandHandler"/> class.
    /// </summary>
    public UpdateProjectTaskStatusCommandHandler(IUnitOfWork unitOfWork, IProjectTaskStatusRepository statusRepository)
    {
        _unitOfWork = unitOfWork;
        _statusRepository = statusRepository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateProjectTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var statusId = request.StatusId.GuidConversion<ProjectTaskStatusId>();
        var status = await _statusRepository.GetByIdAsync(statusId, cancellationToken);
        if (status is null)
        {
            return DomainNotFound.Fail<ProjectTaskStatus>();
        }

        var result = status.EditName(request.Name);
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        _statusRepository.Update(status);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}