using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.ProjectTasks.Queries.GetProjectTaskById;

public sealed record GetProjectTaskByIdQuery(
        Guid ProjectTaskId)
    : IQuery<ProjectTask>;

public class GetProjectTaskById : IQueryHandler<GetProjectTaskByIdQuery, ProjectTask>
{
    private readonly IProjectTaskRepository _taskRepository;

    public GetProjectTaskById(IProjectTaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result<ProjectTask>> Handle(GetProjectTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(new ProjectTaskId(request.ProjectTaskId));
        if (task is null)
        {
            return Result.Fail(new DomainErrors.ProjectTaskErrors.ProjectTaskNotFound());
        }

        return Result.Ok(task);
    }
}