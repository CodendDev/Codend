using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public record struct ProjectTaskStatusId(Guid Value) : IEntityId<Guid>;