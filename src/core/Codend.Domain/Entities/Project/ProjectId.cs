using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public sealed record ProjectId : EntityId<Guid>
{
    public ProjectId()
    {
    }

    public ProjectId(Guid value) : base(value)
    {
    }
}