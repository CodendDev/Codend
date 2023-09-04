using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectRepository
{
    void Add(Project project);
}