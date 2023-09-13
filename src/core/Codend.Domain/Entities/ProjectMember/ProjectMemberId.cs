using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public record struct ProjectMemberId(Guid Value) : IEntityId<Guid>;