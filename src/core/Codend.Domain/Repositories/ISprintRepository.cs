using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface ISprintRepository
{
    Task<Sprint?> GetByIdAsync(SprintId id, CancellationToken cancellationToken);
    void Update(Sprint sprint);
    void Add(Sprint sprint);
    void Remove(Sprint sprint);

    Task<bool> SprintTasksExistsInSprintAsync(
        SprintId sprintId,
        IEnumerable<ISprintTaskId> sprintTaskIds,
        CancellationToken token
    );
}