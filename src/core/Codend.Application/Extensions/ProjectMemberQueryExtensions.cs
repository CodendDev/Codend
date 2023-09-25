using Codend.Domain.Entities;

namespace Codend.Application.Extensions;

internal static class ProjectMemberQueryExtensions
{
    /// <summary>
    /// Gets all projects that user is member of.
    /// </summary>
    internal static IQueryable<ProjectId> GetUserProjectsIds(this IQueryable<ProjectMember> query, UserId userId)
    {
        return query.Where(x => x.MemberId == userId).Select(x => x.ProjectId);
    }
}