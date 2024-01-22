using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Codend.Persistence.Repositories;

internal class SprintRepository : GenericRepository<SprintId, Guid, Sprint>, ISprintRepository
{
    public SprintRepository(CodendApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> SprintTasksExistsInSprintAsync(
        SprintId sprintId,
        IEnumerable<ISprintTaskId> sprintTaskIds,
        CancellationToken token
    )
    {
        var sprintTasks = await Context.Set<SprintProjectTask>()
            .Where(st => st.SprintId == sprintId)
            .ToListAsync(token);

        var anyExists = sprintTasks.Any(
            st => sprintTaskIds.Any(id =>
                (st.TaskId != null && id.Value == st.TaskId.Value) ||
                (st.StoryId != null && id.Value == st.StoryId.Value) ||
                (st.EpicId != null && id.Value == st.EpicId.Value)
            ));

        return anyExists;
    }
}