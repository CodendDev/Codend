using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public sealed record UserId(Guid Value) : IEntityId<Guid, UserId>
{
    public static UserId Create(Guid value) => new(value);
}