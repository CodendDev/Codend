using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectTaskStatusRepository
{
    Task<ProjectTaskStatus?> GetByIdAsync(ProjectTaskStatusId id);

    void Add(ProjectTaskStatus status);

    Task AddRangeAsync(IEnumerable<ProjectTaskStatus> statuses);
}