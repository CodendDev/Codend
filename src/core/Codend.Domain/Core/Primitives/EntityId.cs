using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Core.Primitives;

public abstract class EntityId<TKey> : IEntityId<TKey>
{
    protected EntityId()
    {
    }

    protected EntityId(TKey value)
    {
        _value = value;
    }

    private readonly TKey? _value;

    public TKey Value
    {
        get => _value ?? throw new NullReferenceException("Id cannot be null!");
        init => _value = value;
    }
}