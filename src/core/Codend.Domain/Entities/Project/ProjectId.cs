using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities.Project;

public record struct ProjectId(Guid Value) : IEntityId<Guid>;