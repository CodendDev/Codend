using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public sealed record EpicId : EntityId<Guid>
{
    public EpicId()
    {
    }

    public EpicId(Guid value) : base(value)
    {
    }
}