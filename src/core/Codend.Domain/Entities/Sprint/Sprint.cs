using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public class Sprint : Entity<SprintId>, ISoftDeletableEntity
{
    private Sprint(SprintId id) : base(id)
    {
    }

    public DateTime DeletedOn { get; }
    public bool Deleted { get; }

    public ProjectId ProjectId { get; set; }
    
    public virtual List<ProjectTask> SprintProjectTasks { get; set; }
}