namespace Codend.Domain.Core.Abstractions;

/// <summary>
/// Interface for strongly typed entity identifier.
/// </summary>
/// <typeparam name="TKey">Type of strongly typed value.</typeparam>
public interface IEntityId<TKey>
{
    /// <summary>
    /// Value of <typeparamref name="TKey"/> id.
    /// </summary>
    public TKey Value { get; }
}