using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public record struct ProjectId(Guid Value) : IEntityId<Guid>;