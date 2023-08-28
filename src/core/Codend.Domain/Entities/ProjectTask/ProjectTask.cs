using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities.ProjectTask;

public abstract class ProjectTask : Aggregate<ProjectTaskId>, ISoftDeletableEntity
{
    protected ProjectTask(ProjectTaskId id) : base(id)
    {
    }

    public DateTime DeletedOn { get; }
    public bool Deleted { get; }
}