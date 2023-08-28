using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities.Sprint;

public class Sprint : Entity<SprintId>, ISoftDeletableEntity
{
    public Sprint(SprintId id) : base(id)
    {
    }

    public DateTime DeletedOn { get; }
    public bool Deleted { get; }
}