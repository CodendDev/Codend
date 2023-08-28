using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public class Project : Aggregate<ProjectId>, ISoftDeletableEntity
{
    private Project(ProjectId id) : base(id)
    {
    }

    public DateTime DeletedOnUtc { get; }
    public bool Deleted { get; }

    public virtual Backlog Backlog { get; set; }

    public virtual List<ProjectVersion> ProjectVersions { get; set; }

    public virtual List<Sprint> Sprints { get; set; }
}