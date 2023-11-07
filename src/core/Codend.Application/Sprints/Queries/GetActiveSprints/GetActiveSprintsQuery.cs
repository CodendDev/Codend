using Codend.Application.Core.Abstractions.Common;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Application.Extensions;
using Codend.Contracts.Responses.Sprint;
using Codend.Domain.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Codend.Application.Sprints.Queries.GetActiveSprints;

/// <summary>
/// Query used for retrieving all active sprints in project. 
/// </summary>
/// <param name="ProjectId">If of the project.</param>
public record GetActiveSprintsQuery
(
    ProjectId ProjectId
) : IQuery<IEnumerable<SprintInfoResponse>>;

/// <summary>
/// <see cref="GetActiveSprintsQuery"/> handler.
/// </summary>
public class GetSprintsQueryHandler : IQueryHandler<GetActiveSprintsQuery, IEnumerable<SprintInfoResponse>>
{
    private readonly IQueryableSets _sets;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSprintsQueryHandler"/> class.
    /// </summary>
    public GetSprintsQueryHandler(IQueryableSets sets, IDateTime dateTime)
    {
        _sets = sets;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task<Result<IEnumerable<SprintInfoResponse>>> Handle(
        GetActiveSprintsQuery request,
        CancellationToken cancellationToken)
    {
        var today = _dateTime.UtcNow;

        var sprints = await _sets
            .Queryable<Sprint>()
            .GetProjectSprints(request.ProjectId)
            .Where(s => s.Period.StartDate < today && s.Period.EndDate > today)
            .OrderBy(s => s.Period.EndDate)
            .Select(s =>
                new SprintInfoResponse(s.Id.Value, s.Name.Value, s.Period.StartDate, s.Period.EndDate, s.Goal.Value))
            .ToListAsync(cancellationToken);

        return Result.Ok(sprints.AsEnumerable());
    }
}