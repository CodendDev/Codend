using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public sealed record ProjectMemberId(Guid Value) : IEntityId<Guid, ProjectMemberId>
{
    public static ProjectMemberId Create(Guid value) => new(value);
}