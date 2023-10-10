using AutoMapper;
using AutoMapper.QueryableExtensions;
using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Application.Core.Abstractions.Querying;
using Codend.Application.Extensions;
using Codend.Contracts.Common;
using Codend.Contracts.Responses.Project;
using Codend.Domain.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Codend.Application.Projects.Queries.GetProjects;

/// <summary>
/// Query for retrieving all matching projects, for which user has permissions.
/// </summary>
/// <param name="PageSize">Number of entries on page.</param>
/// <param name="PageIndex">Index of the page.</param>
/// <param name="SortColumn">Column to sort by.</param>
/// <param name="SortOrder">Asc or desc sorting order.</param>
/// <param name="Search">Text to be searched for in project title.</param>
public sealed record GetProjectsQuery(
        int PageIndex,
        int PageSize,
        string? SortColumn = null,
        string? SortOrder = null,
        string? Search = null
    )
    : IQuery<PagedList<ProjectResponse>>, IPageableQuery, ISortableQuery, ITextSearchQuery
{
}

/// <summary>
/// <see cref="GetProjectsQuery"/> Handler.
/// </summary>
public class GetProjectsQueryHandler : IQueryHandler<GetProjectsQuery, PagedList<ProjectResponse>>
{
    private readonly IMapper _mapper;
    private readonly IHttpContextProvider _identityProvider;
    private readonly IQueryableSets _queryableSets;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProjectsQueryHandler"/> class.
    /// </summary>
    public GetProjectsQueryHandler(
        IMapper mapper,
        IHttpContextProvider identityProvider,
        IQueryableSets queryableSets)
    {
        _mapper = mapper;
        _identityProvider = identityProvider;
        _queryableSets = queryableSets;
    }

    /// <inheritdoc />
    public async Task<Result<PagedList<ProjectResponse>>> Handle(GetProjectsQuery query,
        CancellationToken cancellationToken)
    {
        var userId = _identityProvider.UserId;
        var userProjects = _queryableSets.Queryable<ProjectMember>().GetUserProjectsIds(userId);
        var projectsQuery = _queryableSets.Queryable<Project>().GetProjectsWithIds(userProjects);

        var projects = await projectsQuery
            .Search(query.Search)
            .Sort(query, ProjectSortColumnSelector.SortColumnSelector(query.SortColumn))
            .Paginate(query)
            .ProjectTo<ProjectResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var totalCount = await projectsQuery.CountAsync(cancellationToken);

        var result = new PagedList<ProjectResponse>(projects, query.PageIndex, query.PageSize, totalCount);
        return Result.Ok(result);
    }
}