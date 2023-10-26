using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface ISprintProjectTaskRepository
{
    Task AddRangeAsync(IEnumerable<SprintProjectTask> sprintProjectTasks, CancellationToken cancellationToken);
}