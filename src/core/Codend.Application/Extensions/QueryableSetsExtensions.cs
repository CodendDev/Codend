using Codend.Application.Core.Abstractions.Data;
using Codend.Contracts.Responses.Board;
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

    internal static async Task<IEnumerable<BoardTaskResponse>> GetBoardTasksBySprintTasksAsync(
        this IQueryableSets sets,
        IQueryable<SprintProjectTask> sprintTasks,
        CancellationToken cancellationToken
    )
    {
        var boardProjectTasks = await sets
            .Queryable<BaseProjectTask>()
            .Join(
                sprintTasks,
                task => task.Id,
                sprintTask => sprintTask.TaskId,
                (task, sprintTask) => task.ToBoardTaskResponse(sprintTask)
            )
            .ToListAsync(cancellationToken);

        var boardStories = await sets
            .Queryable<Story>()
            .Join(
                sprintTasks,
                task => task.Id,
                sprintTask => sprintTask.StoryId,
                (story, sprintTask) => story.ToBoardTaskResponse(sprintTask)
            )
            .ToListAsync(cancellationToken);

        var boardEpics = await sets
            .Queryable<Epic>()
            .Join(
                sprintTasks,
                epic => epic.Id,
                sprintTask => sprintTask.EpicId,
                (epic, sprintTask) => epic.ToBoardTaskResponse(sprintTask)
            )
            .ToListAsync(cancellationToken);

        return boardProjectTasks.Union(boardStories).Union(boardEpics);
    }
}