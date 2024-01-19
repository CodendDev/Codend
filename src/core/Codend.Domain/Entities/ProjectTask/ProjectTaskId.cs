using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public sealed record ProjectTaskId(Guid Value) : IEntityId<Guid, ProjectTaskId>, ISprintTaskId
{
    public static ProjectTaskId Create(Guid value) => new(value);
}