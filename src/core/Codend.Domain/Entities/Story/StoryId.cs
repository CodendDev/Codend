using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public sealed record StoryId : EntityId<Guid>, ISprintTaskId
{
    public StoryId()
    {
    }

    public StoryId(Guid value) : base(value)
    {
    }
}