using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities.Backlog;

public record struct BacklogId(Guid Value) : IEntityId<Guid>;
