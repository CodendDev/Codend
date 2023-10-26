using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public sealed record SprintProjectTaskId : EntityId<Guid>
{
    public SprintProjectTaskId()
    {
    }

    public SprintProjectTaskId(Guid value) : base(value)
    {
    }
}