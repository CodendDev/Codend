using Codend.Domain.Core.Events;

namespace Codend.Domain.Core.Abstractions;

public interface IAggregate
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}