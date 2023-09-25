using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public Task<int> GetStatusesCountByProjectAsync(ProjectId projectId, CancellationToken cancellationToken)
    {
        var count =
            Context.Set<ProjectTaskStatus>()
                .Where(status => status.ProjectId == projectId)
                .CountAsync(cancellationToken);

        return count;
    }

    public Task<ProjectTaskStatus> GetDefaultStatusInProjectAsync(ProjectId projectId,
        CancellationToken cancellationToken)
    {
        var defaultStatus =
            Context.Set<ProjectTaskStatus>()
                .FirstAsync(status => status.ProjectId == projectId, cancellationToken);

        return defaultStatus;
    }
}