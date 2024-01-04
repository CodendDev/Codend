using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public sealed record ProjectVersionId(Guid Value) : IEntityId<Guid, ProjectVersionId>
{
    public static ProjectVersionId Create(Guid value) => new(value);
}