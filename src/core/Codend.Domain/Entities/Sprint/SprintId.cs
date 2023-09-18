using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public sealed record SprintId : EntityId<Guid>
{
    public SprintId()
    {
    }

    public SprintId(Guid value) : base(value)
    {
    }
}