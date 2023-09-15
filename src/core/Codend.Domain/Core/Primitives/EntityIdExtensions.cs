namespace Codend.Domain.Core.Primitives;

public static class EntityIdExtensions
{
    public static TEntityId? ToKeyGuid<TEntityId>(Guid? value)
        where TEntityId : EntityId<Guid>, new()
    {
        if (value is null)
        {
            return null;
        }

        return new TEntityId { Value = value.Value };
    }
}