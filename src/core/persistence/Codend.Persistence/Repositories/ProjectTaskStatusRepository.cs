using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Persistence.Repositories;

public class ProjectTaskStatusRepository : IProjectTaskStatusRepository
{
    private readonly CodendApplicationDbContext _context;

    public ProjectTaskStatusRepository(CodendApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(ProjectTaskStatus status)
    {
        _context.Add(status);
    }

    public Task AddRangeAsync(IEnumerable<ProjectTaskStatus> statuses)
    {
        return _context.AddRangeAsync(statuses);
    }
}