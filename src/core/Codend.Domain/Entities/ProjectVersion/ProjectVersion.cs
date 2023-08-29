using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;

namespace Codend.Domain.Entities;

public class ProjectVersion : Entity<ProjectVersionId>, ISoftDeletableEntity
{
    private ProjectVersion(ProjectVersionId id) : base(id)
    {
    }

    public DateTime DeletedOnUtc { get; }
    public bool Deleted { get; }

    public ProjectId ProjectId { get; private set; }

    public ProjectVersionName? VersionName { get; private set; }
    
    public ProjectVersionChangelog? Changelog { get; private set; }
}