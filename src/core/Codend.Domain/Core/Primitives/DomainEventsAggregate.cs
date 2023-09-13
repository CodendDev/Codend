using Codend.Domain.Core.Abstractions;

namespace Codend.Domain.Core.Primitives;

/// <inheritdoc cref="Codend.Domain.Core.Abstractions.IDomainEventsAggregate"/> implementation.
public abstract class DomainEventsAggregate<TKey> : Entity<TKey>, IDomainEventsAggregate
{
    /// <inheritdoc />
    protected DomainEventsAggregate(TKey id) : base(id)
    {
    }

    private readonly List<IDomainEvent> _domainEvents = new();

    /// <inheritdoc />
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    /// <inheritdoc />
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Adds event to the event list.
    /// </summary>
    /// <param name="domainEvent">Domain event.</param>
    protected void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}