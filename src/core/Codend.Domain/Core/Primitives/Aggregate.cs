using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Events;

namespace Codend.Domain.Core.Primitives;

public abstract class Aggregate<TKey> : Entity<TKey>, IAggregate
{
    protected Aggregate(TKey id) : base(id)
    {
    }

    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => this._domainEvents;

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void Raise(IDomainEvent domainEvent)
    {
        this._domainEvents.Add(domainEvent);
    }
}