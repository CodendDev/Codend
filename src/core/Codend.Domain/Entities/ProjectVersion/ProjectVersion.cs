using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities.ProjectVersion;

public class ProjectVersion : Entity<ProjectVersionId>, ISoftDeletableEntity
{
    public ProjectVersion(ProjectVersionId id) : base(id)
    {
    }

    public DateTime DeletedOn { get; }
    public bool Deleted { get; }
}