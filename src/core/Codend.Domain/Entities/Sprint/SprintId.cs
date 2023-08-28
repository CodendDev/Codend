using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities.Sprint;

public record struct SprintId(Guid Value) : IEntityId<Guid>;