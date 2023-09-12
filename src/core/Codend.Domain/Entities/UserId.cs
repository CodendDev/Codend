using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public record UserId(Guid Value) : IEntityId<Guid>;