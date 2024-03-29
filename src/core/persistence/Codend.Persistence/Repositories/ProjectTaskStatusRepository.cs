using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Codend.Shared.Infrastructure.Lexorank;
using Microsoft.EntityFrameworkCore;

namespace Codend.Persistence.Repositories;

internal class ProjectTaskStatusRepository
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

    public Task<int> GetStatusesCountByProjectAsync(ProjectId projectId, CancellationToken cancellationToken)
    {
        var count =
            Context.Set<ProjectTaskStatus>()
                .Where(status => status.ProjectId == projectId)
                .CountAsync(cancellationToken);

        return count;
    }

    public Task<ProjectTaskStatusId> GetProjectDefaultStatusIdAsync(
        ProjectId projectId,
        CancellationToken cancellationToken
    )
    {
        var defaultStatusId = Context.Set<Project>()
            .Where(project => project.Id == projectId)
            .Select(project => project.DefaultStatusId)
            .FirstAsync(cancellationToken);

        return defaultStatusId;
    }

    public Task<bool> StatusExistsWithNameAsync(string name, ProjectId projectId, CancellationToken cancellationToken)
    {
        var exists =
            Context.Set<ProjectTaskStatus>()
                .AnyAsync(status => status.Name.Value == name &&
                                    status.ProjectId == projectId,
                    cancellationToken);

        return exists;
    }

    public Task<bool> StatusExistsWithStatusIdAsync(
        ProjectTaskStatusId statusId,
        ProjectId projectId,
        CancellationToken cancellationToken
    )
    {
        var exists =
            Context.Set<ProjectTaskStatus>()
                .AnyAsync(status => status.Id == statusId &&
                                    status.ProjectId == projectId,
                    cancellationToken);
        return exists;
    }

    public Task<Lexorank?> GetLowestStatusInProjectPositionAsync(
        ProjectId projectId,
        CancellationToken cancellationToken
    )
    {
        var lowestPosition = Context.Set<ProjectTaskStatus>().AsNoTracking()
            .Where(projectTaskStatus => projectTaskStatus.Position != null)
            .MaxAsync(projectTaskStatus => projectTaskStatus.Position, cancellationToken);

        return lowestPosition;
    }
}