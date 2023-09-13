using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectMemberRepository
{
    void Add(ProjectMember projectMember);

    Task<ProjectMember?> GetByIdAsync(ProjectMemberId projectMemberId);

    void Remove(ProjectMember projectMember);

    void Update(ProjectMember projectMember);
}