using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Core.Primitives;

public abstract class Entity<TKey> : IEntity<TKey>
{
    protected Entity(TKey id)
    {
        Id = id;
    }

    public TKey Id { get; set; }
}