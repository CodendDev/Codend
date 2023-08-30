using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public record struct UserId(Guid Value) : IEntityId<Guid>;