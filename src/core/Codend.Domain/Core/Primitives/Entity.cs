using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Core.Primitives;

public abstract class Entity<TKey> : IEntity<TKey>, ICreatableEntity
    where TKey : IEntityId
{
    protected Entity(TKey id)
    {
        Id = id;
    }

    public TKey Id { get; private set; }

    public DateTime CreatedOn { get; private set; }
}