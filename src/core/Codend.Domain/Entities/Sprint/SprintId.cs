using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public sealed class SprintId : EntityId<Guid>
{
    public SprintId()
    {
    }

    public SprintId(Guid value) : base(value)
    {
    }
}