using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Entities;

public sealed record EpicId(Guid Value) : IEntityId<Guid>;