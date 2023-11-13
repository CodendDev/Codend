using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Contracts.Responses.Backlog;
using Codend.Domain.Entities;
using FluentResults;

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
) : IQuery<BacklogResponse>;

/// <summary>
/// <see cref="GetAssignableTasksQuery"/> handler.
/// </summary>
public class GetAssignableTasksQueryHandler : IQueryHandler<GetAssignableTasksQuery, BacklogResponse>
{
    /// <inheritdoc />
    public Task<Result<BacklogResponse>> Handle(GetAssignableTasksQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}