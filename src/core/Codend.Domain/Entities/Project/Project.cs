using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities.Project;

public class Project : Aggregate<ProjectId>, ISoftDeletableEntity
{
    public Project(ProjectId id) : base(id)
    {
    }

    public DateTime DeletedOn { get; }
    public bool Deleted { get; }
}