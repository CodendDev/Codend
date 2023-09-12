using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Persistence.Repositories;

public class ProjectTaskStatusRepository
    : GenericRepository<ProjectTaskStatusId, Guid, ProjectTaskStatus>,
        IProjectTaskStatusRepository
{
    public ProjectTaskStatusRepository(CodendApplicationDbContext context) : base(context)
    {
    }

    public Task AddRangeAsync(IEnumerable<ProjectTaskStatus> statuses)
    {
        return Context.AddRangeAsync(statuses);
    }
}