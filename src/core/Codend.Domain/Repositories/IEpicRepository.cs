using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IEpicRepository
{
    /// <summary>
    /// Adds <see cref="epic"/> to the database.
    /// </summary>
    /// <param name="epic">Epic.</param>
    void Add(Epic epic);
    
    /// <summary>
    /// Returns epic with given id or null.
    /// </summary>
    /// <param name="epicId">Id of the epic.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><see cref="Epic"/> or null.</returns>
    Task<Epic?> GetByIdAsync(EpicId epicId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Updates given epic.
    /// </summary>
    /// <param name="epic">Epic be update.</param>
    void Update(Epic epic);
    
    /// <summary>
    /// Removes given epic.
    /// </summary>
    /// <param name="epic">Epic to be deleted.</param>
    void Remove(Epic epic);
    
    /// <summary>
    /// Collects epics with given statusId.
    /// </summary>
    /// <returns>Collection of epics with given statusId.</returns>
    Task<List<Epic>> GetEpicsByStatusId(ProjectTaskStatusId statusId, CancellationToken cancellationToken);
}