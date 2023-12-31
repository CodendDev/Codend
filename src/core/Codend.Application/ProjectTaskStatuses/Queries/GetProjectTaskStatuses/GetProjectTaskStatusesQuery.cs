﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Contracts.Responses.ProjectTaskStatus;
using Codend.Domain.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Codend.Application.ProjectTaskStatuses.Queries.GetProjectTaskStatuses;

/// <summary>
/// Query used for retrieving all project task statuses.
/// </summary>
/// <param name="ProjectId">Id of the project which statuses will be returned.</param>
public sealed record GetProjectTaskStatusesQuery(ProjectId ProjectId)
    : IQuery<IEnumerable<ProjectTaskStatusResponse>>;

/// <summary>
/// <see cref="GetProjectTaskStatusesQueryHandler"/> handler.
/// </summary>
public class GetProjectTaskStatusesQueryHandler
    : IQueryHandler<GetProjectTaskStatusesQuery, IEnumerable<ProjectTaskStatusResponse>>
{
    private readonly IQueryableSets _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProjectTaskStatusesQueryHandler"/> class.
    /// </summary>
    public GetProjectTaskStatusesQueryHandler(
        IQueryableSets context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Result<IEnumerable<ProjectTaskStatusResponse>>> Handle(GetProjectTaskStatusesQuery request,
        CancellationToken cancellationToken)
    {
        var statuses = await _context.Queryable<ProjectTaskStatus>()
            .Where(status => status.ProjectId == request.ProjectId)
            .OrderBy(status => status.Position)
            .ProjectTo<ProjectTaskStatusResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result.Ok(statuses.AsEnumerable());
    }
}