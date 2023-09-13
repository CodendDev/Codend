using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Persistence.Repositories;

public class ProjectMemberRepository : GenericRepository<ProjectMemberId, Guid, ProjectMember>, IProjectMemberRepository
{
    public ProjectMemberRepository(CodendApplicationDbContext context) : base(context)
    {
    }
}