using Codend.Application.Core.Abstractions.Data;
using Codend.Contracts.Responses.Board;
using Codend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Codend.Application.Extensions;

internal static class QueryableSetsExtensions
{
    internal static Task<IEnumerable<BoardTaskResponse>> GetBoardTasksAsyncByProjectId(
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

        return sets.GetBoardTasksAsyncBySprintTasks(sprintTasks, cancellationToken);
    }

    internal static async Task<IEnumerable<BoardTaskResponse>> GetBoardTasksAsyncBySprintTasks(
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
                (task, sprintTask) =>
                    new BoardTaskResponse(
                        task.Id.Value,
                        task.Name.Value,
                        task.StatusId.Value,
                        task.StoryId!.Value,
                        task.Priority.Name,
                        null,
                        sprintTask.Position!.Value
                    )
            )
            .ToListAsync(cancellationToken);

        var boardStories = await sets
            .Queryable<Story>()
            .Join(
                sprintTasks,
                task => task.Id,
                sprintTask => sprintTask.StoryId,
                (task, sprintTask) =>
                    new BoardTaskResponse(
                        task.Id.Value,
                        task.Name.Value,
                        task.StatusId.Value,
                        task.EpicId!.Value,
                        null,
                        null,
                        sprintTask.Position!.Value
                    )
            )
            .ToListAsync(cancellationToken);

        var boardEpics = await sets
            .Queryable<Epic>()
            .Join(
                sprintTasks,
                task => task.Id,
                sprintTask => sprintTask.EpicId,
                (task, sprintTask) =>
                    new BoardTaskResponse(
                        task.Id.Value,
                        task.Name.Value,
                        task.StatusId.Value,
                        null,
                        null,
                        null,
                        sprintTask.Position!.Value
                    )
            )
            .ToListAsync(cancellationToken);

        return boardProjectTasks.Union(boardStories).Union(boardEpics);
    }
}