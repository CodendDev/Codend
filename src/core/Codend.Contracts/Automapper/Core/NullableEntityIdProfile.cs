using AutoMapper;
using Codend.Domain.Core.Abstractions;

namespace Codend.Contracts.Automapper.Core;

/// <summary>
/// Nullable <see cref="IEntityId{TKey}"/> automapper profile.
/// </summary>
public sealed class NullableEntityIdProfile : Profile
{
    /// <summary>
    /// Initializes maps for nullable <see cref="IEntityId{TKey}"/> class.
    /// </summary>
    public NullableEntityIdProfile()
    {
        CreateMap<IEntityId<Guid>?, Guid?>().ConvertUsing(strongId => strongId != null ? strongId.Value : null);
    }
}