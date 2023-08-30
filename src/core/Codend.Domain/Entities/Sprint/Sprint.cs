using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;

namespace Codend.Domain.Entities;

public class Sprint : Entity<SprintId>, ISoftDeletableEntity
{
    private Sprint(SprintId id) : base(id)
    {
    }
    
    public SprintPeriod Period { get; private set; }
    public SprintGoal? Goal { get; private set; }
    public ProjectId ProjectId { get; private set; }
    public virtual List<ProjectTask> SprintProjectTasks { get; private set; }
    
    public DateTime DeletedOnUtc { get; private set; }
    public bool Deleted { get; }
}