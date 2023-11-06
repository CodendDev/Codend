using System.Collections;
using Codend.Contracts.Responses.Board;
using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Application.Extensions;

internal static class SprintTaskExtensions
{
    internal static BoardTaskResponse ToBoardTaskResponse<T>(this T task, SprintProjectTask projectTask)
        where T : ISprintTask =>
        new(
            task.SprintTaskId,
            task.SprintTaskType,
            task.SprintTaskName,
            task.SprintTaskStatusId,
            task.SprintTaskRelatedTaskId,
            task.SprintTaskPriority,
            null,
            projectTask.Position?.Value
        );

    internal static BoardTaskResponse ToBoardTaskResponseWithAvatar(this BoardTaskResponse task, string? avatarUrl) =>
        task with { AssigneeAvatar = avatarUrl };

    internal static IEnumerable<BoardTaskResponse> JoinSprintTaskWith<T>(
        this IEnumerable<SprintProjectTask> sprintTasksQuery,
        IEnumerable<T> tasks
    )
        where T : class, ISprintTask
    {
        return tasks
            .Join(
                sprintTasksQuery,
                projectTask => projectTask.SprintTaskId,
                sprintProjectTask => sprintProjectTask.SprintTaskId,
                (task, sprintProjectTask) => task.ToBoardTaskResponse(sprintProjectTask)
            );
    }
}