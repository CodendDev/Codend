using System.Data;
using AutoMapper;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Contracts.Responses.ProjectTask;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.ProjectTasks.Queries.GetProjectTaskById;

public sealed record GetProjectTaskByIdQuery(
        Guid ProjectTaskId)
    : IQuery<AbstractProjectTaskResponse>;

public class GetProjectTaskById : IQueryHandler<GetProjectTaskByIdQuery, AbstractProjectTaskResponse>
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IProjectTaskStatusRepository _statusRepository;
    private readonly IMapper _mapper;

    public GetProjectTaskById(
        IProjectTaskRepository taskRepository,
        IProjectTaskStatusRepository statusRepository,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
        _statusRepository = statusRepository;
    }

    public async Task<Result<AbstractProjectTaskResponse>> Handle(
        GetProjectTaskByIdQuery request,
        CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(new ProjectTaskId(request.ProjectTaskId));
        if (task is null)
        {
            return Result.Fail(new DomainErrors.ProjectTaskErrors.ProjectTaskNotFound());
        }

        var dto = _mapper.Map<AbstractProjectTaskResponse>(task);
        var status = await _statusRepository.GetByIdAsync(task.StatusId);
        if (status is null)
        {
            throw new NoNullAllowedException("ProjectStatus can't be null.");
        }

        dto.Status = status.Name.Value;
        return Result.Ok(dto);
    }
}