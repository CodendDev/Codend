using Codend.Application.Core.Abstractions.Data;
using Codend.Contracts.Responses.Board;
using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Codend.Application.Extensions;

internal static class QueryableSetsExtensions
{
    internal static Task<IEnumerable<BoardTaskResponse>> GetBoardTasksByProjectIdAsync(
        this IQueryableSets sets,
        ProjectId projectId,
        CancellationToken cancellationToken
    )
    {
        var sprints = sets
            .Queryable<Sprint>()
            .Where(sprint => sprint.ProjectId == projectId);

        var sprintTasks = sets
            .Queryable<SprintProjectTask>()
            .Where(task => sprints.Any(s => s.Id == task.SprintId));

        return sets.GetBoardTasksBySprintTasksAsync(sprintTasks, cancellationToken);
    }

    private static async Task<IEnumerable<BoardTaskResponse>> GetBoardTasksBySprintTasksAsync(
        this IQueryableSets sets,
        IQueryable<SprintProjectTask> sprintTasks,
        CancellationToken cancellationToken
    )
    {
        var boardProjectTasks =
            await sets.GetBoardTasksBySprintTasksAsync<BaseProjectTask>(sprintTasks, cancellationToken);
        var boardStories = await sets.GetBoardTasksBySprintTasksAsync<Story>(sprintTasks, cancellationToken);
        var boardEpics = await sets.GetBoardTasksBySprintTasksAsync<Epic>(sprintTasks, cancellationToken);

        return boardProjectTasks.Union(boardStories).Union(boardEpics);
    }

    internal static async Task<IEnumerable<BoardTaskResponse>> GetBoardTasksBySprintTasksAsync<T>(
        this IQueryableSets sets,
        IEnumerable<SprintProjectTask> sprintTasks,
        CancellationToken cancellationToken
    ) where T : class, ISprintTask, IEntity
    {
        var set = await sets
            .Queryable<T>()
            .ToListAsync(cancellationToken);

        return set
            .Join(
                sprintTasks,
                task => task.SprintTaskId,
                sprintTask => sprintTask.SprintTaskId,
                (task, sprintTask) => task.ToBoardTaskResponse(sprintTask)
            );
    }
}