using Codend.Application.Core.Abstractions.Common;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Application.Extensions;
using Codend.Application.Extensions.ef;
using Codend.Contracts.Responses.Board;
using Codend.Contracts.Responses.Sprint;
using Codend.Domain.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;

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
    public async Task<Result<SprintsResponse>> Handle(GetSprintsQuery request, CancellationToken cancellationToken)
    {
        var boardTasks = await _sets.GetBoardTasksByProjectIdAsync(request.ProjectId, cancellationToken);

        var sprints = await _sets
            .Queryable<Sprint>()
            .GetProjectSprints(request.ProjectId)
            .ToListAsync(cancellationToken);

        var sprintBoards = sprints
            .GroupJoin(
                _sets.Queryable<SprintProjectTask>(),
                sprint => sprint.Id,
                task => task.SprintId,
                (sprint, tasks) => new { sprint, Board = new BoardResponse(FindBoardTasks(tasks, boardTasks)) }
            );

        var sprintResponses = sprintBoards.Select(
            sprintBoard =>
                new SprintResponse
                (
                    sprintBoard.sprint.Id.Value,
                    sprintBoard.sprint.Name.Value,
                    sprintBoard.sprint.Period.StartDate,
                    sprintBoard.sprint.Period.EndDate,
                    sprintBoard.sprint.Goal.Value,
                    sprintBoard.Board
                )
        );

        var activeSprints = sprints
            .Where(s => s.IsSprintActiveThisDay(_dateTime.UtcNow))
            .Select(s => s.Id.Value);

        return Result.Ok(new SprintsResponse(activeSprints, sprintResponses));
    }

    private static IEnumerable<BoardTaskResponse> FindBoardTasks(
        IEnumerable<SprintProjectTask> sprintTask,
        IEnumerable<BoardTaskResponse> tasks
    ) =>
        tasks
            .Where(t =>
                sprintTask.Any(
                    st =>
                        (st.TaskId != null && t.Id == st.TaskId.Value) ||
                        (st.StoryId != null && t.Id == st.StoryId.Value) ||
                        (st.EpicId != null && t.Id == st.EpicId.Value)
                )
            );
}