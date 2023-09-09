using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectTaskRepository
{
    void Add(ProjectTask task);

    Task<ProjectTask?> GetByIdAsync(ProjectTaskId taskId);

    void Remove(ProjectTask task);

    void Update(ProjectTask task);

    /// <summary>
    /// Checks whether project exists and checks whether status is contained by given project.
    /// </summary>
    /// <returns>true id status is valid for this project, otherwise false</returns>
    bool ProjectTaskIsValid(ProjectId projectId, ProjectTaskStatusId statusId);
}