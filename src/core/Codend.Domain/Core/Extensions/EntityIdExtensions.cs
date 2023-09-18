namespace Codend.Domain.Core.Primitives;

public static class EntityIdExtensions
{
    /// <summary>
    /// Converts nullable guid to any <see cref="EntityId{TKey}"/> where TKey is <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">Nullable <see cref="Guid"/> to be converted.</param>
    /// <typeparam name="TEntityId">Any <see cref="EntityId{TKey}"/> id.</typeparam>
    /// <returns><see cref="TEntityId"/> object with guid as value or null if guid is null.</returns>
    public static TEntityId? GuidConversion<TEntityId>(this Guid? guid)
        where TEntityId : EntityId<Guid>, new()
        => guid?.GuidConversion<TEntityId>();

    /// <summary>
    /// Converts guid to o any <see cref="EntityId{TKey}"/> where TKey is <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid"><see cref="Guid"/> to be converted.</param>
    /// <typeparam name="TEntityId">Any <see cref="EntityId{TKey}"/> id.</typeparam>
    /// <returns><see cref="TEntityId"/> object with guid as value.</returns>
    public static TEntityId GuidConversion<TEntityId>(this Guid guid)
        where TEntityId : EntityId<Guid>, new()
        => new() { Value = guid };
}