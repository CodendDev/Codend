using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public record ProjectId(Guid Value) : IEntityId<Guid, ProjectId>
{
    public static ProjectId Create(Guid value)
    {
        return new ProjectId(value);
    }
}