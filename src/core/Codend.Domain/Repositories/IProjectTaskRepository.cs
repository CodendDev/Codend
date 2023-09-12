using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectTaskRepository
{
    void Add(ProjectTaskBase task);

    Task<ProjectTaskBase?> GetByIdAsync(ProjectTaskId taskId);

    void Remove(ProjectTaskBase task);

    void Update(ProjectTaskBase task);

    /// <summary>
    /// Checks whether project exists and checks whether status is contained by given project.
    /// </summary>
    /// <returns>true id status is valid for this project, otherwise false</returns>
    bool ProjectTaskIsValid(ProjectId projectId, ProjectTaskStatusId statusId);
}