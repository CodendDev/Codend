using Codend.Domain.Entities;

namespace Codend.Application.Extensions;

internal static class SprintQueryExtensions
{
    internal static IQueryable<Sprint> GetProjectSprints(this IQueryable<Sprint> queryable, ProjectId projectId) =>
        queryable.Where(s => s.ProjectId == projectId);
}