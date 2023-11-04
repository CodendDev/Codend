using AutoMapper;
using AutoMapper.QueryableExtensions;
using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Application.Core.Abstractions.Querying;
using Codend.Application.Core.Abstractions.Services;
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
using Codend.Domain.Core.Primitives;
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
public sealed record GetBoardQuery
(
    ProjectId ProjectId,
    SprintId SprintId,
    UserId? AssigneeId
) : IQuery<BoardResponse>;

/// <summary>
/// <see cref="GetBoardQuery"/> Handler.
/// </summary>
public class GetBoardQueryHandler : IQueryHandler<GetBoardQuery, BoardResponse>
{
    private readonly IMapper _mapper;
    private readonly IQueryableSets _queryableSets;
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProjectsQueryHandler"/> class.
    /// </summary>
    public GetBoardQueryHandler(
        IMapper mapper,
        IQueryableSets queryableSets,
        IUserService userService
    )
    {
        _mapper = mapper;
        _queryableSets = queryableSets;
        _userService = userService;
    }

    private async Task<Result<BoardResponse>> HandleAssigneeNotNull(
        GetBoardQuery query,
        CancellationToken cancellationToken
    )
    {
        var sprintTasksQuery =
            _queryableSets.Queryable<SprintProjectTask>()
                .Where(task => task.SprintId == query.SprintId);

        var projectTasksQuery = await
            _queryableSets.Queryable<BaseProjectTask>()
                .Where(baseProjectTask =>
                    baseProjectTask.ProjectId == query.ProjectId &&
                    baseProjectTask.AssigneeId == query.AssigneeId
                )
                .Join(sprintTasksQuery,
                    projectTask => projectTask.Id,
                    sprintProjectTask => sprintProjectTask.TaskId,
                    (projectTask, sprintProjectTask) => new
                    {
                        Id = projectTask.Id,
                        Name = projectTask.Name,
                        StatusId = projectTask.StatusId,
                        StoryId = projectTask.StoryId,
                        Priority = projectTask.Priority,
                        Position = sprintProjectTask.Position,
                        AssigneeId = projectTask.AssigneeId
                    }
                ).ToListAsync(cancellationToken);

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var userIds = projectTasksQuery
            .Where(boardTask => boardTask.AssigneeId is not null)
            .Select(boardTask => boardTask.AssigneeId)
            .ToList();
        var users = await _userService.GetUsersByIdsAsync(userIds);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var projectTasks = projectTasksQuery
            .Select(task =>
                new BoardTaskResponse
                (
                    task.Id.Value,
                    task.Name.Value,
                    task.StatusId.Value,
                    task.StoryId?.Value,
                    task.Priority.Name,
                    task.AssigneeId is not null ? users.Single(u => u.Id == task.AssigneeId?.Value).ImageUrl : null,
                    task.Position?.Value
                )
            );

        return Result.Ok(new BoardResponse(projectTasks));
    }

    /// <inheritdoc />
    public async Task<Result<BoardResponse>> Handle(
        GetBoardQuery query,
        CancellationToken cancellationToken
    )
    {
        if (query.AssigneeId is not null)
        {
            return await HandleAssigneeNotNull(query, cancellationToken);
        }

        var sprintTasksQuery =
            _queryableSets.Queryable<SprintProjectTask>()
                .Where(task => task.SprintId == query.SprintId);

        var projectTasksQuery = await
            _queryableSets.Queryable<BaseProjectTask>()
            .Where(baseProjectTask => baseProjectTask.ProjectId == query.ProjectId)
            .Join(sprintTasksQuery,
                projectTask => projectTask.Id,
                sprintProjectTask => sprintProjectTask.TaskId,
                (projectTask, sprintProjectTask) => new
                {
                    Id = projectTask.Id,
                    Name = projectTask.Name,
                    StatusId = projectTask.StatusId,
                    StoryId = projectTask.StoryId,
                    Priority = projectTask.Priority,
                    Position = sprintProjectTask.Position,
                    AssigneeId = projectTask.AssigneeId
                }
            ).ToListAsync(cancellationToken);

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var userIds = projectTasksQuery
            .Where(boardTask => boardTask.AssigneeId is not null)
            .Select(boardTask => boardTask.AssigneeId)
            .ToList();
        var users = await _userService.GetUsersByIdsAsync(userIds);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var storiesQuery = await _queryableSets.Queryable<Story>()
            .Where(story => story.ProjectId == query.ProjectId)
            .Join(sprintTasksQuery,
                story => story.Id,
                sprintProjectTask => sprintProjectTask.StoryId,
                (story, sprintProjectTask) => new
                {
                    Id = story.Id,
                    Name = story.Name,
                    StatusId = story.StatusId,
                    EpicId = story.EpicId,
                    Position = sprintProjectTask.Position
                }
            )
            .ToListAsync(cancellationToken);

        var epicsQuery = await _queryableSets.Queryable<Epic>()
            .Where(epic => epic.ProjectId == query.ProjectId)
            .Join(sprintTasksQuery,
                epic => epic.Id,
                sprintProjectTask => sprintProjectTask.EpicId,
                (epic, sprintProjectTask) => new
                {
                    Id = epic.Id,
                    Name = epic.Name,
                    StatusId = epic.StatusId,
                    Position = sprintProjectTask.Position
                }
            )
            .ToListAsync(cancellationToken);

        var projectTasks = projectTasksQuery
            .Select(task =>
                new BoardTaskResponse
                (
                    task.Id.Value,
                    task.Name.Value,
                    task.StatusId.Value,
                    task.StoryId?.Value,
                    task.Priority.Name,
                    task.AssigneeId is not null ? users.Single(u => u.Id == task.AssigneeId?.Value).ImageUrl : null,
                    task.Position?.Value
                )
            );
        var storyTasks = storiesQuery
            .Select(story =>
                new BoardTaskResponse
                (
                    story.Id.Value,
                    story.Name.Value,
                    story.StatusId.Value,
                    story.EpicId?.Value,
                    null,
                    null,
                    story.Position?.Value
                )
            );
        var epicTasks = epicsQuery
            .Select(epic =>
                new BoardTaskResponse
                (
                    epic.Id.Value,
                    epic.Name.Value,
                    epic.StatusId.Value,
                    null,
                    null,
                    null,
                    epic.Position?.Value
                )
            );
        var merged = projectTasks.Union(storyTasks).Union(epicTasks);

        return Result.Ok(new BoardResponse(merged));
    }
}