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
}