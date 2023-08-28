using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public record struct ProjectVersionId(Guid Value) : IEntityId<Guid>;
