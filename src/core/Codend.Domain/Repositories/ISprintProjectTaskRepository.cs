using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface ISprintProjectTaskRepository
{
    IEnumerable<SprintProjectTask> GetRangeBySprintIdAndProjectTaskIds(
        SprintId sprintId,
        IEnumerable<ProjectTaskId> taskIds
    );

    Task AddRangeAsync(IEnumerable<SprintProjectTask> sprintProjectTasks, CancellationToken cancellationToken);

    void RemoveRange(IEnumerable<SprintProjectTask> sprintProjectTasks);
}