using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectTaskRepository
{
    void Add(ProjectTask project);

    Task<ProjectTask?> GetByIdAsync(ProjectTaskId projectId);

    void Remove(ProjectTask project);
}