using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities.User;

public record struct UserId(Guid Value) : IEntityId<Guid>;