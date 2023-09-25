using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectTaskStatusRepository
{
    Task<ProjectTaskStatus?> GetByIdAsync(ProjectTaskStatusId id, CancellationToken cancellationToken);

    void Add(ProjectTaskStatus status);

    void Remove(ProjectTaskStatus status);

    Task AddRangeAsync(IEnumerable<ProjectTaskStatus> statuses);

    Task<int> GetStatusesCountByProjectAsync(ProjectId projectId, CancellationToken cancellationToken);

    Task<ProjectTaskStatus> GetDefaultStatusInProjectAsync(ProjectId projectId, CancellationToken cancellationToken);
}