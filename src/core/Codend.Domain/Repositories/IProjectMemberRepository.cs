using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectMemberRepository
{
    void Add(ProjectMember projectMember);

    void Remove(ProjectMember projectMember);

    void Update(ProjectMember projectMember);

    public void UpdateRange(IEnumerable<ProjectMember> entities);

    /// <summary>
    /// Searches for ProjectMember entity with user and member Id.
    /// </summary>
    /// <param name="projectId">Project Id.</param>
    /// <param name="memberId">Member Id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>First matching ProjectMember found or null.</returns>
    Task<ProjectMember?> GetByProjectAndMemberId(ProjectId projectId, UserId memberId, CancellationToken cancellationToken);

    Task<ICollection<ProjectMember>> GetByMembersIdAsync(UserId memberId, CancellationToken cancellationToken);
    
    Task<bool> IsProjectMember(UserId memberId, ProjectId projectId, CancellationToken cancellationToken);

    Task<int> GetProjectMembersCount(ProjectId projectId);
}