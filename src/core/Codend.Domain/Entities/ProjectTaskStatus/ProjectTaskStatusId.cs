using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public sealed record ProjectTaskStatusId(Guid Value) : IEntityId<Guid, ProjectTaskStatusId>

{
    public static ProjectTaskStatusId Create(Guid value) => new(value);
}