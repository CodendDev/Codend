using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public sealed class UserId : EntityId<Guid>
{
    public UserId()
    {
    }

    public UserId(Guid value) : base(value)
    {
    }
}