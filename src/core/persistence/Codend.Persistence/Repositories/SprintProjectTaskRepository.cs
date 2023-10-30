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

    public Task<List<SprintProjectTask>> GetRangeBySprintIdAndProjectTaskIds(
        SprintId sprintId,
        IEnumerable<ProjectTaskId> taskIds)
    {
        var sprintProjectTasks = Context
            .Set<SprintProjectTask>()
            .Where(sprintProjectTask =>
                sprintProjectTask.SprintId == sprintId &&
                taskIds.Any(id => sprintProjectTask.TaskId == id))
            .ToListAsync();

        return sprintProjectTasks;
    }
}