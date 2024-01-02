using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Application.Projects.Queries.GetBacklog;
using Codend.Contracts.Responses.Sprint;
using Codend.Domain.Entities;
using FluentResults;
using MediatR;
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
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAssignableTasksQueryValidator"/> class.
    /// </summary>
    public GetAssignableTasksQueryHandler(IQueryableSets sets, IMediator mediator)
    {
        _sets = sets;
        _mediator = mediator;
    }

    /// 💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀
    public async Task<Result<AssignableTasksResponse>> Handle(
        GetAssignableTasksQuery request,
        CancellationToken cancellationToken
    )
    {
        var query = new GetBacklogQuery(request.ProjectId);
        var backlogResult = await _mediator.Send(query, cancellationToken);

        if (backlogResult.IsFailed)
        {
            return backlogResult.ToResult();
        }

        var backlog = backlogResult.Value.Tasks.ToList();

        var sprintTasks = await _sets
            .Queryable<SprintProjectTask>()
            .Where(task =>
                task.SprintId == request.SprintId
            )
            .ToListAsync(cancellationToken);

        var assignableTasks = backlog
            .Where(t => sprintTasks.All(st => st.SprintTaskId != t.Id));

        return Result.Ok(
            new AssignableTasksResponse(
                assignableTasks.Select(t =>
                    new AssignableTaskResponse(t.Id, t.Name, t.TaskType, t.StatusName)
                )
            )
        );
    }
}