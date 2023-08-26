using Codend.Domain.Core.Events;

namespace Codend.Domain.Core.Primitives;

public abstract class AggregateRoot<Tkey> : Entity<Tkey>
{
    protected AggregateRoot(Tkey id) : base(id)
    {
    }

    private readonly List<DomainEvent> _domainEvents = new();

    public IReadOnlyCollection<DomainEvent> DomainEvents => this._domainEvents;

    protected void Raise(DomainEvent domainEvent)
    {
        this._domainEvents.Add(domainEvent);
    }
}