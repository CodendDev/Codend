using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;

namespace Codend.Domain.Entities;

public class Project : Aggregate<ProjectId>, ISoftDeletableEntity
{
    private Project(ProjectId id) : base(id)
    {
    }

    public DateTime DeletedOnUtc { get; }
    public bool Deleted { get; }

    public virtual List<ProjectTask> ProjectTasks { get; private set; }

    public virtual List<ProjectVersion> ProjectVersions { get; private set; }

    public virtual List<Sprint> Sprints { get; private set; }

    public ProjectName ProjectName { get; private set; }

    public ProjectDescription? ProjectDescription { get; private set; }
}