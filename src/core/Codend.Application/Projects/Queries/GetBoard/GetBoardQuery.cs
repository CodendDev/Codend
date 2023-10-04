using AutoMapper;
using AutoMapper.QueryableExtensions;
using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Application.Core.Abstractions.Querying;
using Codend.Application.Extensions;
using Codend.Application.Projects.Queries.GetProjects;
using Codend.Contracts.Automapper.Entities.ProjectTask;
using Codend.Contracts.Common;
using Codend.Contracts.Responses.Board;
using Codend.Contracts.Responses.Epic;
using Codend.Contracts.Responses.Project;
using Codend.Contracts.Responses.ProjectTask;
using Codend.Contracts.Responses.Story;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask.Bugfix;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Projects.Queries.GetBoard;

/// <summary>
/// Query for retrieving all board information for project, including tasks, epic and stories.
/// </summary>
/// <param name="ProjectId">Id of the project for which board will be returned.</param>
public sealed record GetBoardQuery(ProjectId ProjectId) : IQuery<BoardResponse>;

/// <summary>
/// <see cref="GetBoardQuery"/> Handler.
/// </summary>
public class GetBoardQueryHandler : IQueryHandler<GetBoardQuery, BoardResponse>
{
    private readonly IMapper _mapper;
    private readonly IUserIdentityProvider _identityProvider;
    private readonly IQueryableSets _queryableSets;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProjectsQueryHandler"/> class.
    /// </summary>
    public GetBoardQueryHandler(
        IMapper mapper,
        IUserIdentityProvider identityProvider,
        IQueryableSets queryableSets)
    {
        _mapper = mapper;
        _identityProvider = identityProvider;
        _queryableSets = queryableSets;
    }

    /// <inheritdoc />
    public async Task<Result<BoardResponse>> Handle(GetBoardQuery query,
        CancellationToken cancellationToken)
    {
        var userId = _identityProvider.UserId;
        if (await _queryableSets.Queryable<ProjectMember>().IsUserMember(userId, query.ProjectId) is false)
        {
            return DomainNotFound.Fail<Project>();
        }

        var projectTasks = await _queryableSets.Queryable<BaseProjectTask>()
            .Where(baseProjectTask => baseProjectTask.ProjectId == query.ProjectId)
            .ProjectTo<BoardProjectTaskResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var stories = await _queryableSets.Queryable<Story>()
            .Where(story => story.ProjectId == query.ProjectId)
            .ProjectTo<BoardStoryResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var epics = await _queryableSets.Queryable<Epic>()
            .Where(story => story.ProjectId == query.ProjectId)
            .ProjectTo<BoardEpicResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result.Ok(new BoardResponse(projectTasks, stories, epics));
    }
}