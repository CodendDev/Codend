using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Codend.Shared.Infrastructure.Lexorank;
using Microsoft.EntityFrameworkCore;

namespace Codend.Persistence.Repositories;

public class SprintProjectTaskRepository
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

    public Task<SprintProjectTask?> GetByTaskIdAsync(ISprintTaskId entityId, CancellationToken cancellationToken)
    {
        var sprintTasks = Context.Set<SprintProjectTask>();
        var sprintProjectTask = entityId switch
        {
            ProjectTaskId id => sprintTasks.SingleOrDefaultAsync(task => task.TaskId == id, cancellationToken),
            StoryId id => sprintTasks.SingleOrDefaultAsync(task => task.SprintId == id, cancellationToken),
            EpicId id => sprintTasks.SingleOrDefaultAsync(task => task.EpicId == id, cancellationToken),
            _ => throw new ArgumentException("SprintProjectTaskId is not any of supported types")
        };
        return sprintProjectTask;
    }

    public Task<Lexorank?> GetHighestTaskInSprintPosition(SprintId sprintId)
    {
        var highestPosition = Context.Set<SprintProjectTask>().AsNoTracking()
            .Where(sprintProjectTask => sprintProjectTask.Position != null)
            .MinAsync(sprintProjectTask => sprintProjectTask.Position);

        return highestPosition;
    }
}