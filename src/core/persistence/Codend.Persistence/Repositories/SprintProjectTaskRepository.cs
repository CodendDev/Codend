using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Codend.Shared.Infrastructure.Lexorank;
using Microsoft.EntityFrameworkCore;

namespace Codend.Persistence.Repositories;

internal class SprintProjectTaskRepository
    : GenericRepository<SprintProjectTaskId, Guid, SprintProjectTask>,
        ISprintProjectTaskRepository
{
    public SprintProjectTaskRepository(CodendApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<SprintProjectTask>> GetRangeBySprintIdAndProjectTaskIdsAsync(
        SprintId sprintId,
        IEnumerable<ISprintTaskId> taskIds
    )
    {
        var sprintTasks = await Context
            .Set<SprintProjectTask>()
            .Where(sprintProjectTask => sprintProjectTask.SprintId == sprintId)
            .ToListAsync();

        var sprintProjectTasks = sprintTasks.Where(st =>
            taskIds.Any(id =>
                (st.TaskId != null && id.Value == st.TaskId.Value) ||
                (st.StoryId != null && id.Value == st.StoryId.Value) ||
                (st.EpicId != null && id.Value == st.EpicId.Value)));

        return sprintProjectTasks.ToList();
    }

    public Task<SprintProjectTask?> GetBySprintTaskIdAsync(ISprintTaskId entityId, string sprintTaskType,
        CancellationToken cancellationToken)
    {
        var sprintTasks = Context.Set<SprintProjectTask>();
        // ️💀
        var taskId = entityId.Value.GuidConversion<ProjectTaskId>();
        var storyId = entityId.Value.GuidConversion<StoryId>();
        var epicId = entityId.Value.GuidConversion<EpicId>();
        var sprintProjectTask = sprintTaskType.ToLower() switch
        {
            "base" or "bugfix" or "task" => sprintTasks.SingleOrDefaultAsync(
                task => Equals(task.TaskId, taskId), cancellationToken),
            "story" => sprintTasks.SingleOrDefaultAsync(
                task => Equals(task.StoryId, storyId), cancellationToken),
            "epic" => sprintTasks.SingleOrDefaultAsync(task => Equals(task.EpicId, epicId),
                cancellationToken),
            _ => throw new ArgumentException("SprintProjectTaskId is not any of supported types")
        };
        return sprintProjectTask;
    }

    public Task<Lexorank?> GetHighestTaskInSprintPositionAsync(SprintId sprintId, CancellationToken cancellationToken)
    {
        var highestPosition = Context.Set<SprintProjectTask>().AsNoTracking()
            .Where(sprintProjectTask => sprintProjectTask.Position != null)
            .MinAsync(sprintProjectTask => sprintProjectTask.Position, cancellationToken);

        return highestPosition;
    }
}