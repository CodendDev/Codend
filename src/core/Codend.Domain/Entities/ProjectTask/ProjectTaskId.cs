using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities.ProjectTask;

public record struct ProjectTaskId(Guid Value) : IEntityId<Guid>;