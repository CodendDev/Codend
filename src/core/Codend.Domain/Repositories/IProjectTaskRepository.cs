using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectTaskRepository
{
    /// <summary>
    /// Adds <see cref="BaseProjectTask"/> to the database.
    /// </summary>
    /// <param name="task">Task.</param>
    void Add(BaseProjectTask task);

    /// <summary>
    /// Returns task with given id or null.
    /// </summary>
    /// <param name="taskId">Id of the Task.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><see cref="BaseProjectTask"/> or null.</returns>
    Task<BaseProjectTask?> GetByIdAsync(ProjectTaskId taskId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes given task.
    /// </summary>
    /// <param name="task">Task to be deleted.</param>
    void Remove(BaseProjectTask task);

    /// <summary>
    /// Updates given task.
    /// </summary>
    /// <param name="task">Task be update.</param>
    void Update(BaseProjectTask task);

    /// <summary>
    /// Updates range.
    /// </summary>
    /// <param name="tasks">Tasks to update.</param>
    void UpdateRange(IEnumerable<BaseProjectTask> tasks);

    /// <summary>
    /// Checks whether project exists and checks whether status is contained by given project.
    /// </summary>
    /// <returns>true id status is valid for this project, otherwise false</returns>
    bool ProjectTaskStatusIsValid(ProjectId projectId, ProjectTaskStatusId statusId);

    /// <summary>
    /// Collects tasks which belongs to a given story.
    /// </summary>
    /// <param name="storyId">StoryId.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of tasks which belongs to the <see cref="Story"/>.</returns>
    Task<List<BaseProjectTask>> GetStoryTasksAsync(StoryId storyId, CancellationToken cancellationToken);

    /// <summary>
    /// Collects tasks which has given status.
    /// </summary>
    /// <param name="statusId">StatusId.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of tasks which has given status.</returns>
    Task<List<BaseProjectTask>> GetTasksByStatusIdAsync(ProjectTaskStatusId statusId, CancellationToken cancellationToken);
}