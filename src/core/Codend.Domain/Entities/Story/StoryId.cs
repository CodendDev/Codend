using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public sealed record StoryId : EntityId<Guid>
{
    public StoryId()
    {
    }

    public StoryId(Guid value) : base(value)
    {
    }
}