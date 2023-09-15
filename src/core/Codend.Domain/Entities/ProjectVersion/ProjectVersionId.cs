using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public sealed class ProjectVersionId : EntityId<Guid>
{
    public ProjectVersionId()
    {
    }

    public ProjectVersionId(Guid value) : base(value)
    {
    }
}