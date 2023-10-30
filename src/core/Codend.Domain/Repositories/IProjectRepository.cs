using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectRepository
{
    void Add(Project project);

    Task<Project?> GetByIdAsync(ProjectId projectId);

    void Remove(Project project);

    void Update(Project project);

    Task<bool> Exists(ProjectId projectId);

    Task<bool> ProjectContainsEpic(ProjectId projectId, EpicId epicId);

    Task<int> CountSprintTasksInProjectAsync<TEntity, TKey>(
        ProjectId projectId,
        IEnumerable<ISprintTaskId> taskIds,
        CancellationToken token
    )
        where TKey : ISprintTaskId
        where TEntity : class, IProjectOwnedEntity, IEntity<TKey>;
}