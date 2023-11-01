using Codend.Domain.Entities;
using Codend.Shared.Infrastructure.Lexorank;

namespace Codend.Domain.Repositories;

public interface IProjectTaskStatusRepository
{
    Task<ProjectTaskStatus?> GetByIdAsync(ProjectTaskStatusId id, CancellationToken cancellationToken);

    void Add(ProjectTaskStatus status);

    void Remove(ProjectTaskStatus status);

    void Update(ProjectTaskStatus status);

    Task AddRangeAsync(IEnumerable<ProjectTaskStatus> statuses);

    Task<int> GetStatusesCountByProjectAsync(ProjectId projectId, CancellationToken cancellationToken);

    Task<ProjectTaskStatusId> GetProjectDefaultStatusIdAsync(ProjectId projectId, CancellationToken cancellationToken);

    Task<bool> StatusExistsWithNameAsync(string name, ProjectId projectId, CancellationToken cancellationToken);

    Task<bool> StatusExistsWithStatusIdAsync(ProjectTaskStatusId statusId, ProjectId projectId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Searches for and returns projectTaskStatus with lowest position, where lowest mean highest lexicographical ranking (last status).
    /// </summary>
    /// <param name="projectId">Project for which statuses will be considered.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><see cref="Lexorank"/> position of lowest status or null if no matches.</returns>
    Task<Lexorank?> GetLowestStatusInProjectPositionAsync(ProjectId projectId, CancellationToken cancellationToken);
}