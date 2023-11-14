using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Application.Extensions.ef;
using Codend.Contracts.Responses.Sprint;
using Codend.Domain.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Codend.Application.Sprints.Queries.GetAssignableTasks;

/// <summary>
/// Query used for retrieving tasks assignable to given sprint.
/// </summary>
/// <param name="ProjectId">Id of the project.</param>
/// <param name="SprintId">Id of the sprint.</param>
public record GetAssignableTasksQuery
(
    ProjectId ProjectId,
    SprintId SprintId
) : IQuery<AssignableTasksResponse>;

/// <summary>
/// <see cref="GetAssignableTasksQuery"/> handler.
/// </summary>
public class GetAssignableTasksQueryHandler : IQueryHandler<GetAssignableTasksQuery, AssignableTasksResponse>
{
    private readonly IQueryableSets _sets;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAssignableTasksQueryValidator"/> class.
    /// </summary>
    public GetAssignableTasksQueryHandler(IQueryableSets sets)
    {
        _sets = sets;
    }

    /// 💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀
    public async Task<Result<AssignableTasksResponse>> Handle(
        GetAssignableTasksQuery request,
        CancellationToken cancellationToken
    )
    {
        var sprints = _sets
            .Queryable<Sprint>()
            .Where(sprint => sprint.ProjectId == request.ProjectId);
        var sprintTasks = await _sets
            .Queryable<SprintProjectTask>()
            .Where(task =>
                task.SprintId != request.SprintId &&
                sprints.Any(s => s.Id == task.SprintId)
            )
            .ToListAsync(cancellationToken);

        var boardTasks = await _sets.GetBoardTasksBySprintTasksAsync(request.ProjectId, sprintTasks, cancellationToken);

        var statuses = await _sets.Queryable<ProjectTaskStatus>()
            .Where(status => status.ProjectId == request.ProjectId)
            .ToDictionaryAsync(status => status.Id.Value, status => status.Name.Value, cancellationToken);

        var res = new AssignableTasksResponse(
            boardTasks
                .Select(x =>
                    new AssignableTaskResponse(
                        x.Id,
                        x.Name,
                        x.TaskType,
                        statuses[x.StatusId]
                    )
                )
        );

        return Result.Ok(res);
    }
}