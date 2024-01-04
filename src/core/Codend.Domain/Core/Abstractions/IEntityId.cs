namespace Codend.Domain.Core.Abstractions;

/// <summary>
/// Interface for strongly typed entity identifier.
/// </summary>
/// <typeparam name="TKey">Type of strongly typed value.</typeparam>
public interface IEntityId<TKey> : IEntityId
{
    /// <summary>
    /// Value of <typeparamref name="TKey"/> id.
    /// </summary>
    TKey Value { get; }
}

/// <summary>
/// Interface for strongly typed entity identifier with static Create method used to guid conversion.
/// </summary>
/// <typeparam name="TKey">Type of strongly typed value.</typeparam>
/// <typeparam name="TSelf">Self type</typeparam>
public interface IEntityId<TKey, TSelf> : IEntityId<TKey>
    where TSelf : IEntityId<TKey, TSelf>
{
    static abstract TSelf Create(Guid value);
}

public interface IEntityId
{
}