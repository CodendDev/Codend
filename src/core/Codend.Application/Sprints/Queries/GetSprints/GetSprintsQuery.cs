using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Contracts.Responses.Sprint;
using Codend.Domain.Entities;
using FluentResults;
using MediatR;

namespace Codend.Application.Sprints.Queries.GetSprints;

/// <summary>
/// Query used for retrieving all sprints in project. 
/// </summary>
/// <param name="ProjectId">If of the project.</param>
public record GetSprintsQuery
(
    ProjectId ProjectId
) : IQuery<SprintsResponse>;

/// <summary>
/// <see cref="GetSprintsQuery"/> handler.
/// </summary>
public class GetSprintsQueryHandler : IQueryHandler<GetSprintsQuery, SprintsResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSprintsQueryHandler"/> class.
    /// </summary>
    public GetSprintsQueryHandler()
    {
    }

    /// <inheritdoc />
    public Task<Result<SprintsResponse>> Handle(GetSprintsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}