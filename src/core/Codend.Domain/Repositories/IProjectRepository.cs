using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectRepository
{
    void Add(Project project);

    Task<Project?> GetByIdAsync(ProjectId projectId);

    void Remove(Project project);

    void Update(Project project);

    Task<bool> Exists(ProjectId projectId);

    Task<bool> ProjectContainsEpic(ProjectId projectId, EpicId epicId);
}