using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
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
}