using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Codend.Persistence.Repositories;

public class ProjectRepository : GenericRepository<ProjectId, Guid, Project>, IProjectRepository
{
    public ProjectRepository(CodendApplicationDbContext context) : base(context)
    {
    }

    public Task<bool> ProjectContainsEpic(ProjectId projectId, EpicId epicId)
    {
        var contains = Context.Set<Epic>()
            .AnyAsync(e => e.Id == epicId && e.ProjectId == projectId);

        return contains;
    }
}