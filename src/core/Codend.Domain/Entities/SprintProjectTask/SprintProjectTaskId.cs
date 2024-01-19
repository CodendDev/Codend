using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public sealed record SprintProjectTaskId(Guid Value) : IEntityId<Guid, SprintProjectTaskId>
{
    public static SprintProjectTaskId Create(Guid value) => new(value);
}