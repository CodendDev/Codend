using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IProjectMemberRepository
{
    void Add(ProjectMember projectMember);

    Task<ProjectMember?> GetByIdAsync(ProjectMemberId projectMemberId);

    void Remove(ProjectMember projectMember);

    void Update(ProjectMember projectMember);

    /// <summary>
    /// Searches for ProjectMember entity with user and member Id.
    /// </summary>
    /// <param name="projectId">Project Id.</param>
    /// <param name="memberId">Member Id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>First matching ProjectMember found or null.</returns>
    Task<ProjectMember?> GetByProjectAndMemberId(ProjectId projectId, UserId memberId, CancellationToken cancellationToken);
}