using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public record struct SprintId(Guid Value) : IEntityId<Guid>;