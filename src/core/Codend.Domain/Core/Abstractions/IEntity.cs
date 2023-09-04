namespace Codend.Domain.Core.Abstractions;

/// <summary>
/// Base entity interface, with <typeparamref name="TKey"/> type as Id.
/// </summary>
/// <typeparam name="TKey">Type of entity identifier.</typeparam>
public interface IEntity<TKey>
{
    /// <summary>
    /// Entity identifier.
    /// </summary>
    public TKey Id { get; }
}