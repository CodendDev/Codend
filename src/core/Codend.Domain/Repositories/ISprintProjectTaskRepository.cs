using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;
using Codend.Shared.Infrastructure.Lexorank;

namespace Codend.Domain.Repositories;

public interface ISprintProjectTaskRepository
{
    Task<List<SprintProjectTask>> GetRangeBySprintIdAndProjectTaskIdsAsync(
        SprintId sprintId,
        IEnumerable<ISprintTaskId> taskIds
    );

    public Task<SprintProjectTask?> GetBySprintTaskIdAsync(ISprintTaskId entityId, CancellationToken cancellationToken);

    Task AddRangeAsync(IEnumerable<SprintProjectTask> sprintProjectTasks, CancellationToken cancellationToken);

    void RemoveRange(IEnumerable<SprintProjectTask> sprintProjectTasks);

    public void Update(SprintProjectTask sprintProjectTask);

    /// <summary>
    /// Searches for and returns task with highest position, where highest means lowest lexicographical ranking.
    /// </summary>
    /// <param name="sprintId">Sprint for which tasks will be considered.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><see cref="Lexorank"/> position of highest task or null if no matches.</returns>
    Task<Lexorank?> GetHighestTaskInSprintPositionAsync(SprintId sprintId, CancellationToken cancellationToken);
}