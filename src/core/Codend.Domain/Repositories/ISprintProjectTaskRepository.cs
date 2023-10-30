using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface ISprintProjectTaskRepository
{
    Task<List<SprintProjectTask>> GetRangeBySprintIdAndProjectTaskIdsAsync(
        SprintId sprintId,
        IEnumerable<ISprintTaskId> taskIds
    );

    Task AddRangeAsync(IEnumerable<SprintProjectTask> sprintProjectTasks, CancellationToken cancellationToken);

    void RemoveRange(IEnumerable<SprintProjectTask> sprintProjectTasks);
}