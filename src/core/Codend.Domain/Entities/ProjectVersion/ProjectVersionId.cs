using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities.ProjectVersion;

public record struct ProjectVersionId(Guid Value) : IEntityId<Guid>;
