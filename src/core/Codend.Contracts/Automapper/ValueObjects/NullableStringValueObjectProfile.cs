using AutoMapper;
using Codend.Domain.ValueObjects.Primitives;

namespace Codend.Contracts.Automapper.ValueObjects;

/// <summary>
/// <see cref="NullableStringValueObject"/> automapper profile.
/// </summary>
public sealed class NullableStringValueObjectProfile : Profile
{
    /// <summary>
    /// Initializes maps for <see cref="NullableStringValueObject"/> class.
    /// </summary>
    public NullableStringValueObjectProfile()
    {
        CreateMap<NullableStringValueObject, string?>().ConvertUsing(valueObject => valueObject.Value);
    }
}