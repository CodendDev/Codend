using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Codend.Persistence.Repositories;

public class ProjectMemberRepository : GenericRepository<ProjectMemberId, Guid, ProjectMember>, IProjectMemberRepository
{
    public ProjectMemberRepository(CodendApplicationDbContext context) : base(context)
    {
    }
    
    public Task<ProjectMember?> GetByProjectAndMemberId(ProjectId projectId, UserId memberId, CancellationToken cancellationToken)
    {
        return Context.Set<ProjectMember>().FirstOrDefaultAsync(projectMember =>
            projectMember.ProjectId.Value.Equals(projectId.Value) &&
            projectMember.MemberId.Value.Equals(memberId.Value), cancellationToken: cancellationToken);
    }
}