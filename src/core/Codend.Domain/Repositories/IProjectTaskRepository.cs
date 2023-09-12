using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectTaskRepository
{
    void Add(BaseProjectTask task);

    Task<BaseProjectTask?> GetByIdAsync(ProjectTaskId taskId);

    void Remove(BaseProjectTask task);

    void Update(BaseProjectTask task);

    /// <summary>
    /// Checks whether project exists and checks whether status is contained by given project.
    /// </summary>
    /// <returns>true id status is valid for this project, otherwise false</returns>
    bool ProjectTaskIsValid(ProjectId projectId, ProjectTaskStatusId statusId);
}