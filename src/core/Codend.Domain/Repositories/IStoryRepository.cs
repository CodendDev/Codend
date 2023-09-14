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
    /// <returns><see cref="Story"/> or null.</returns>
    Task<Story?> GetByIdAsync(StoryId storyId);

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
}