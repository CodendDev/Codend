using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public abstract class ProjectTask : Aggregate<ProjectTaskId>, ISoftDeletableEntity
{
    protected ProjectTask(ProjectTaskId id) : base(id)
    {
    }

    public DateTime DeletedOnUtc { get; }
    public bool Deleted { get; }

    public BacklogId BacklogId { get; set; }
}