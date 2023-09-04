namespace Codend.Domain.Core.Abstractions;

/// <summary>
/// Base entity interface, with <typeparamref name="TKey"/> type as Id.
/// </summary>
/// <typeparam name="TKey">Type of entity identifier.</typeparam>
public interface IEntity<TKey> : IEntity
{
    /// <summary>
    /// Entity identifier.
    /// </summary>
    public TKey Id { get; }
}

/// <summary>
/// Base entity interface. 
/// </summary>
public interface IEntity
{
}