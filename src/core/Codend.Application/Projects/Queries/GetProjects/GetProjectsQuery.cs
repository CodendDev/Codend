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
    private readonly IHttpContextProvider _contextProvider;
    private readonly IQueryableSets _queryableSets;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProjectsQueryHandler"/> class.
    /// </summary>
    public GetProjectsQueryHandler(
        IHttpContextProvider contextProvider,
        IQueryableSets queryableSets)
    {
        _contextProvider = contextProvider;
        _queryableSets = queryableSets;
    }

    /// <inheritdoc />
    public async Task<Result<PagedList<ProjectResponse>>> Handle(GetProjectsQuery query,
        CancellationToken cancellationToken)
    {
        var userId = _contextProvider.UserId;
        var projectsQuery = _queryableSets.Queryable<Project>();

        var projectsResponseQuery = _queryableSets.Queryable<ProjectMember>()
            .GetForUser(userId)
            .Join(
                projectsQuery,
                projectMember => projectMember.ProjectId,
                project => project.Id,
                (projectMember, project) => new
                {
                    project.Id,
                    project.Name,
                    project.Description,
                    project.OwnerId,
                    projectMember.IsFavourite
                }
            );
        var searchedProjects = query.Search is null
            ? projectsResponseQuery
            : projectsResponseQuery
                .Where(project => project.Name.Value.ToLower().Contains(query.Search.ToLower()));

        var projects = await searchedProjects
            .OrderByDescending(project => project.IsFavourite)
            .Paginate(query)
            .ToListAsync(cancellationToken);

        var projectsResponse = projects.Select(x =>
            new ProjectResponse(x.Id.Value, x.Name.Value, x.Description.Value, x.OwnerId.Value, x.IsFavourite));

        var totalCount = await projectsResponseQuery.CountAsync(cancellationToken);

        var result = new PagedList<ProjectResponse>(projectsResponse, query.PageIndex, query.PageSize, totalCount);
        return Result.Ok(result);
    }
}