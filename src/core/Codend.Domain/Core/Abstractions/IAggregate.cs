namespace Codend.Domain.Core.Abstractions;

public interface IAggregate
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}