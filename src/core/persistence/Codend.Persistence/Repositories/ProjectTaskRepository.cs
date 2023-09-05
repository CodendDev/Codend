using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Persistence.Repositories;

public class ProjectTaskRepository : IProjectTaskRepository
{
    private readonly CodendApplicationDbContext _context;

    public ProjectTaskRepository(CodendApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(ProjectTask task)
    {
        _context.Add(task);
    }
}