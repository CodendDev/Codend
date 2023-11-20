using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.Sprints.Commands.AssignTasks;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using MediatR;
using static Codend.Domain.Core.Errors.DomainErrors.General;
using static Codend.Domain.Core.Errors.DomainErrors.ProjectTaskStatus;

namespace Codend.Application.Epics.Commands.CreateEpic;

/// <summary>
/// Command user for creating epic. 
/// </summary>
/// <param name="Name">Epic name.</param>
/// <param name="Description">Epic description.</param>
/// <param name="ProjectId">Epic projectId.</param>
/// <param name="StatusId">Epic statusId.</param>
/// <param name="SprintId">Id of the sprint to which epic will be assigned.</param>
public sealed record CreateEpicCommand
(
    string Name,
    string Description,
    ProjectId ProjectId,
    ProjectTaskStatusId? StatusId,
    SprintId? SprintId
) : ICommand<Guid>;

/// <summary>
/// <see cref="CreateEpicCommand"/> handler.
/// </summary>
public class CreateEpicCommandHandler : ICommandHandler<CreateEpicCommand, Guid>
{
    private readonly IEpicRepository _epicRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTaskStatusRepository _statusRepository;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEpicCommandHandler"/> class.
    /// </summary>
    public CreateEpicCommandHandler(
        IEpicRepository epicRepository,
        IUnitOfWork unitOfWork,
        IProjectRepository projectRepository,
        IProjectTaskStatusRepository statusRepository,
        IMediator mediator)
    {
        _epicRepository = epicRepository;
        _unitOfWork = unitOfWork;
        _projectRepository = projectRepository;
        _statusRepository = statusRepository;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(CreateEpicCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project is null)
        {
            return DomainNotFound.Fail<Project>();
        }

        if (request.StatusId is not null &&
            await _statusRepository.StatusExistsWithStatusIdAsync(
                request.StatusId,
                request.ProjectId,
                cancellationToken) is false)
        {
            return Result.Fail(new InvalidStatusId());
        }

        var epicResult = Epic.Create(
            request.Name,
            request.Description,
            request.ProjectId,
            request.StatusId ?? project.DefaultStatusId);
        if (epicResult.IsFailed)
        {
            return epicResult.ToResult();
        }

        var epic = epicResult.Value;
        _epicRepository.Add(epic);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Assign epic to sprint if sprint id provided.
        if (request.SprintId is null) return Result.Ok(epic.Id.Value);
        var result = await _mediator.Send(new SprintAssignTasksCommand(request.SprintId, new[] { epic.Id }),
            cancellationToken);

        return result.IsFailed ? result : Result.Ok(epic.Id.Value);
    }
}