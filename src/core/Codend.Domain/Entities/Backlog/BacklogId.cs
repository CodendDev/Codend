using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public record struct BacklogId(Guid Value) : IEntityId<Guid>;
