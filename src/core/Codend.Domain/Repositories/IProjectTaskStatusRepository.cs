using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectTaskStatusRepository
{
    void Add(ProjectTaskStatus status);
    
    Task AddRangeAsync(IEnumerable<ProjectTaskStatus> statuses);
}