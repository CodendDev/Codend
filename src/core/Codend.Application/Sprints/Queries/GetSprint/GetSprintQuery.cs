using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Application.Extensions.ef;
using Codend.Contracts.Responses.Board;
using Codend.Contracts.Responses.Sprint;
using Codend.Domain.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Sprints.Queries.GetSprint;

/// <summary>
/// Query used for retrieving single sprint.
/// </summary>
/// <param name="ProjectId">Id of the project.</param>
/// <param name="SprintId">Id of the sprint.</param>
public record GetSprintQuery
(
    ProjectId ProjectId,
    SprintId SprintId
) : IQuery<SprintResponse>;

/// <summary>
/// <see cref="GetSprintQuery"/> handler;
/// </summary>
public class GetSprintQueryHandler : IQueryHandler<GetSprintQuery, SprintResponse>
{
    private readonly IQueryableSets _sets;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSprintQueryHandler"/> class.
    /// </summary>
    public GetSprintQueryHandler(IQueryableSets sets)
    {
        _sets = sets;
    }

    /// <inheritdoc />
    public async Task<Result<SprintResponse>> Handle(GetSprintQuery request, CancellationToken cancellationToken)
    {
        var sprint = await _sets
            .Queryable<Sprint>()
            .SingleOrDefaultAsync(s => s.Id == request.SprintId, cancellationToken);

        if (sprint is null)
        {
            return DomainNotFound.Fail<Sprint>();
        }

        var boardTasks =
            await _sets.GetBoardTasksBySprintIdAsync(request.ProjectId, request.SprintId, cancellationToken);

        var result = new SprintResponse
        (
            sprint.Id.Value,
            sprint.Name.Value,
            sprint.Period.StartDate,
            sprint.Period.EndDate,
            sprint.Goal.Value,
            new BoardResponse(boardTasks)
        );

        return Result.Ok(result);
    }
}