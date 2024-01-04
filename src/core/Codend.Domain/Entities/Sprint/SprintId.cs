using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public sealed record SprintId(Guid Value) : IEntityId<Guid, SprintId>
{
    public static SprintId Create(Guid value) => new(value);
}