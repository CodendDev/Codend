using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectRepository
{
    void Add(Project project);

    Task<Project?> GetByIdAsync(ProjectId projectId);

    void Remove(Project project);

    void Update(Project project);
}