using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public sealed record ProjectMemberId : EntityId<Guid>
{
    public ProjectMemberId()
    {
    }

    public ProjectMemberId(Guid value) : base(value)
    {
    }
}