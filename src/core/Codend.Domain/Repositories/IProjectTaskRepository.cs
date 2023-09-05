using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectTaskRepository
{
    void Add(ProjectTask project);
}