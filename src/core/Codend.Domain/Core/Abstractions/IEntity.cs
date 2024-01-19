namespace Codend.Domain.Core.Abstractions;

/// <summary>
/// Base entity interface, with <typeparamref name="TStrongKey"/> type as Id.
/// </summary>
/// <typeparam name="TStrongKey">Type of entity identifier.</typeparam>
public interface IEntity<TStrongKey> : IEntity
    where TStrongKey : IEntityId
{
    /// <summary>
    /// Entity identifier.
    /// </summary>
    public TStrongKey Id { get; }
}

/// <summary>
/// Base entity interface. 
/// </summary>
public interface IEntity
{
}