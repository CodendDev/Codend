using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Persistence.Repositories;

public class ProjectRepository : GenericRepository<ProjectId, Guid, Project>, IProjectRepository
{
    public ProjectRepository(CodendApplicationDbContext context) : base(context)
    {
    }
}