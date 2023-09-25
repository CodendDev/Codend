using Codend.Domain.Entities;

namespace Codend.Application.Extensions;

internal static class ProjectQueryExtensions
{
    internal static IQueryable<Project> GetProjectsWithIds(this IQueryable<Project> query,
        IEnumerable<ProjectId> projectIds)
    {
        return query.Where(x => projectIds.Contains(x.Id));
    }

    internal static IQueryable<Project> Search(this IQueryable<Project> query, string? text)
    {
        return text == null ? query 
            : query.Where(x => x.Name.Value.ToLower().Contains(text.ToLower()));
    }
}