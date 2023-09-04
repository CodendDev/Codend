using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Persistence.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly CodendApplicationDbContext _context;

    public ProjectRepository(CodendApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Project project)
    {
        _context.Add(project);
    }
}