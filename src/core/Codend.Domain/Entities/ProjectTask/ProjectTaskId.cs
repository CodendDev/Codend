using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public record struct ProjectTaskId(Guid Value) : IEntityId<Guid>;