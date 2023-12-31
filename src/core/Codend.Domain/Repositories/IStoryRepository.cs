using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

/// <summary>
/// <see cref="Story"/> repository.
/// </summary>
public interface IStoryRepository
{
    /// <summary>
    /// Adds <see cref="Story"/> to the database.
    /// </summary>
    /// <param name="story">Story.</param>
    void Add(Story story);

    /// <summary>
    /// Returns story with given id or null.
    /// </summary>
    /// <param name="storyId">Id of the task.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><see cref="Story"/> or null.</returns>
    Task<Story?> GetByIdAsync(StoryId storyId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes given story.
    /// </summary>
    /// <param name="story">Story to be deleted.</param>
    void Remove(Story story);

    /// <summary>
    /// Updates given story.
    /// </summary>
    /// <param name="story">Story be update.</param>
    void Update(Story story);

    /// <summary>
    /// Returns collection of stories which belongs to the given epic.
    /// </summary>
    Task<List<Story>> GetStoriesByEpicIdAsync(EpicId epicId, CancellationToken cancellationToken);

    /// <summary>
    /// Updates range.
    /// </summary>
    /// <param name="stories">Stories to update.</param>
    void UpdateRange(IEnumerable<Story> stories);

    /// <summary>
    /// Collects stories with given status.
    /// </summary>
    /// <returns>List of stories which has given status.</returns>
    Task<List<Story>> GetStoriesByStatusIdAsync(ProjectTaskStatusId statusId, CancellationToken cancellationToken);
}