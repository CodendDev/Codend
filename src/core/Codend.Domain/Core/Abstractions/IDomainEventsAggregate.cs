namespace Codend.Domain.Core.Abstractions;

/// <summary>
/// Domain events aggregate.
/// </summary>
public interface IDomainEventsAggregate
{
    /// <summary>
    /// List of aggregate domain events.
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Clears all domain events in this aggregate.
    /// </summary>
    void ClearDomainEvents();
}