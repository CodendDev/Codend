using AutoMapper;
using Codend.Domain.Core.Abstractions;

namespace Codend.Contracts.Automapper.Core;

/// <summary>
/// <see cref="IEntityId{TKey}"/> automapper profile.
/// </summary>
public sealed class EntityIdProfile : Profile
{
    /// <summary>
    /// Initializes maps for <see cref="IEntityId{TKey}"/> class.
    /// </summary>
    public EntityIdProfile()
    {
        CreateMap<IEntityId<Guid>, Guid>().ConvertUsing(strongId => strongId.Value);
    }
}