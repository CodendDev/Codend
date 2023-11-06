using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Application.Core.Abstractions.Services;
using Codend.Application.Extensions;
using Codend.Application.Projects.Queries.GetProjects;
using Codend.Contracts.Responses.Board;
using Codend.Domain.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Codend.Application.Projects.Queries.GetBoard;

/// <summary>
/// Query for retrieving all board information for project, including tasks, epic and stories.
/// </summary>
/// <param name="ProjectId">Id of the project for which board will be returned.</param>
/// <param name="SprintId">Id of the sprint for which board will be returned.</param>
/// <param name="AssigneeId">Id of the assignee for which board will be returned.</param>
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
    private readonly IQueryableSets _queryableSets;
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProjectsQueryHandler"/> class.
    /// </summary>
    public GetBoardQueryHandler(IQueryableSets queryableSets, IUserService userService)
    {
        _queryableSets = queryableSets;
        _userService = userService;
    }

    /// <inheritdoc />
    public async Task<Result<BoardResponse>> Handle(GetBoardQuery query, CancellationToken cancellationToken)
    {
        var sprintTasksQuery = _queryableSets
            .Queryable<SprintProjectTask>()
            .Where(task => task.SprintId == query.SprintId);

        var projectTasks = await HandleProjectTasks(sprintTasksQuery, query, cancellationToken);

        // Story and epic cannot be assigned to any user
        if (query.AssigneeId is not null)
        {
            return Result.Ok(new BoardResponse(projectTasks));
        }

        var storyTasks =
            await _queryableSets.GetBoardTasksBySprintTasksAsync<Story>(sprintTasksQuery, cancellationToken);
        var epicTasks =
            await _queryableSets.GetBoardTasksBySprintTasksAsync<Epic>(sprintTasksQuery, cancellationToken);
        var merged = projectTasks.Union(storyTasks).Union(epicTasks);

        return Result.Ok(new BoardResponse(merged));
    }

    private async Task<IEnumerable<BoardTaskResponse>> HandleProjectTasks(
        IEnumerable<SprintProjectTask> sprintTasksQuery,
        GetBoardQuery query,
        CancellationToken cancellationToken
    )
    {
        var tasks = await _queryableSets
            .Queryable<BaseProjectTask>()
            .Where(baseProjectTask =>
                baseProjectTask.ProjectId != query.ProjectId ||
                query.AssigneeId == null || baseProjectTask.AssigneeId == query.AssigneeId
            )
            .ToListAsync(cancellationToken);

        // Select AssigneeId to assign avatar to assignee
        var projectTasksQuery = sprintTasksQuery
            .JoinSprintTaskWith(tasks)
            .Select(
                boardTask =>
                    new
                    {
                        boardTask,
                        tasks.Single(t => t.Id.Value == boardTask.Id).AssigneeId
                    }
            )
            .ToList();

        // Fetch all needed users
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var userIds = projectTasksQuery
            .Where(boardTask => boardTask.AssigneeId is not null)
            .Select(boardTask => boardTask.AssigneeId)
            .ToList();
        var users = await _userService.GetUsersByIdsAsync(userIds);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types. 

        // Assign avatars for board tasks
        return projectTasksQuery
            .Select(
                task =>
                    task.boardTask.ToBoardTaskResponseWithAvatar(
                        task.AssigneeId is not null
                            ? users.Single(u => u.Id == task.AssigneeId?.Value).ImageUrl
                            : null
                    )
            );
    }
}