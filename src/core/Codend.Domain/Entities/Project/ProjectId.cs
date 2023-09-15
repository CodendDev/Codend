using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public sealed class ProjectId : EntityId<Guid>
{
    public ProjectId()
    {
    }

    public ProjectId(Guid value) : base(value)
    {
    }
}