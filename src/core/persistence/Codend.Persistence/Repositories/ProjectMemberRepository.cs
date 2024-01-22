using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Codend.Persistence.Repositories;

internal class ProjectMemberRepository : GenericRepository<ProjectMemberId, Guid, ProjectMember>, IProjectMemberRepository
{
    public ProjectMemberRepository(CodendApplicationDbContext context) : base(context)
    {
    }

    public Task<ProjectMember?> GetByProjectAndMemberId(ProjectId projectId, UserId memberId,
        CancellationToken cancellationToken)
    {
        return Context.Set<ProjectMember>()
            .SingleOrDefaultAsync(projectMember =>
                    Equals(projectMember.ProjectId, projectId) &&
                    Equals(projectMember.MemberId, memberId),
                cancellationToken);
    }

    public async Task<ICollection<ProjectMember>> GetByMembersIdAsync(UserId memberId,
        CancellationToken cancellationToken)
    {
        return await Context.Set<ProjectMember>()
            .Where(projectMember =>
                projectMember.MemberId == memberId
            )
            .ToListAsync(cancellationToken);
    }

    public Task<bool> IsProjectMember(UserId memberId, ProjectId projectId, CancellationToken cancellationToken)
    {
        var isMember = Context.Set<ProjectMember>()
            .AnyAsync(projectMember =>
                    projectMember.MemberId == memberId &&
                    projectMember.ProjectId == projectId,
                cancellationToken);

        return isMember;
    }

    public Task<int> GetProjectMembersCount(ProjectId projectId)
    {
        return Context.Set<ProjectMember>()
            .CountAsync(x => x.ProjectId == projectId);
    }
}