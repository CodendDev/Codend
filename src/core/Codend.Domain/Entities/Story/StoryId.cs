using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public sealed record StoryId(Guid Value) : IEntityId<Guid, StoryId>, ISprintTaskId
{
    public static StoryId Create(Guid value) => new(value);
}