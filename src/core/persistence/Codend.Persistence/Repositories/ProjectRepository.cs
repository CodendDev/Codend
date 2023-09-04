using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public Task<Project?> GetByIdAsync(ProjectId projectId)
    {
        var project = _context
            .Set<Project>()
            .SingleOrDefaultAsync(project => project.Id == projectId);

        return project;
    }

    public void Remove(Project project)
    {
        _context.Set<Project>().Remove(project);
    }

    public void Update(Project project)
    {
        _context.Set<Project>().Update(project);
    }
}