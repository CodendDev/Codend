using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Persistence.Repositories;

public class ProjectTaskRepository : GenericRepository<ProjectTaskId, Guid, ProjectTask>, IProjectTaskRepository
{
    public ProjectTaskRepository(CodendApplicationDbContext context) : base(context)
    {
    }
}