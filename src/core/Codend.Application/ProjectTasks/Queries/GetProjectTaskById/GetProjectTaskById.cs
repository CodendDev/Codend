using System.Data;
using AutoMapper;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Contracts.Responses.ProjectTask;
using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask.Bugfix;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.ProjectTasks.Queries.GetProjectTaskById;

/// <summary>
/// Command for getting <see cref="BaseProjectTask"/> by its id. 
/// </summary>
/// <param name="ProjectTaskId">Task id.</param>
public sealed record GetProjectTaskByIdQuery
(
    Guid ProjectTaskId
) : IQuery<BaseProjectTaskResponse>;

/// <summary>
/// <see cref="GetProjectTaskByIdQuery"/> handler.
/// </summary>
public class GetProjectTaskByIdQueryHandler : IQueryHandler<GetProjectTaskByIdQuery, BaseProjectTaskResponse>
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IProjectTaskStatusRepository _statusRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructs <see cref="GetProjectTaskByIdQueryHandler"/>.
    /// </summary>
    /// <param name="taskRepository"><see cref="BaseProjectTask"/> repository.</param>
    /// <param name="statusRepository"><see cref="ProjectTaskStatus"/> repository.</param>
    /// <param name="mapper"><see cref="IMapper"/> instance.</param>
    public GetProjectTaskByIdQueryHandler(
        IProjectTaskRepository taskRepository,
        IProjectTaskStatusRepository statusRepository,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
        _statusRepository = statusRepository;
    }

    private BaseProjectTaskResponse Map(BaseProjectTask task)
    {
        var dto = task switch
        {
            BugfixProjectTask => _mapper.Map<BugfixProjectTaskResponse>(task),
            _ => _mapper.Map<BaseProjectTaskResponse>(task)
        };

        return dto;
    }

    /// <inheritdoc />
    /// <exception cref="NoNullAllowedException">Throws when task status doesn't exist in database.</exception>
    public async Task<Result<BaseProjectTaskResponse>> Handle(
        GetProjectTaskByIdQuery request,
        CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(new ProjectTaskId(request.ProjectTaskId), cancellationToken);
        if (task is null)
        {
            return DomainNotFound.Fail<BaseProjectTask>();
        }

        var status = await _statusRepository.GetByIdAsync(task.StatusId, cancellationToken);
        if (status is null)
        {
            throw new NoNullAllowedException("ProjectStatus can't be null.");
        }

        var dto = Map(task);
        dto.Status = status.Name.Value;
        return Result.Ok(dto);
    }
}