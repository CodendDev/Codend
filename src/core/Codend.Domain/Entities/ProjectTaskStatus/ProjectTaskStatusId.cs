using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public sealed record ProjectTaskStatusId : EntityId<Guid>
{
    public ProjectTaskStatusId()
    {
    }

    public ProjectTaskStatusId(Guid value) : base(value)
    {
    }
}