using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public sealed class ProjectTaskId : EntityId<Guid>
{
    public ProjectTaskId()
    {
    }

    public ProjectTaskId(Guid value) : base(value)
    {
    }
}