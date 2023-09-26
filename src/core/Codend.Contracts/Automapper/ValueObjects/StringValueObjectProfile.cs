using AutoMapper;
using Codend.Domain.ValueObjects.Primitives;

namespace Codend.Contracts.Automapper.ValueObjects;

/// <summary>
/// <see cref="StringValueObject"/> automapper profile.
/// </summary>
public sealed class StringValueObjectProfile : Profile
{
    /// <summary>
    /// Initializes maps for <see cref="StringValueObject"/> class.
    /// </summary>
    public StringValueObjectProfile()
    {
        CreateMap<StringValueObject, string>().ConvertUsing(valueObject => valueObject.Value);
    }
}