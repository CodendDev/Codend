using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public sealed record ProjectTaskId : EntityId<Guid>, ISprintTaskId
{
    public ProjectTaskId()
    {
    }

    public ProjectTaskId(Guid value) : base(value)
    {
    }
}