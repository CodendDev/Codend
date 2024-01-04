using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public sealed record EpicId(Guid Value) : IEntityId<Guid, EpicId>, ISprintTaskId
{
    public static EpicId Create(Guid value) => new(value);
}