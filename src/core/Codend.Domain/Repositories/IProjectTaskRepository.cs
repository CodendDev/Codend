using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectTaskRepository
{
    void Add(ProjectTask project);

    Task<ProjectTask?> GetByIdAsync(ProjectTaskId projectId);

    void Remove(ProjectTask project);

    /// <summary>
    /// Checks whether project exists and checks whether status is contained by given project.
    /// </summary>
    /// <returns>true id status is valid for this project, otherwise false</returns>
    bool ProjectTaskIsValid(ProjectId projectId, ProjectTaskStatusId statusId);
}