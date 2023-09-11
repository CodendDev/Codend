using AutoMapper;
using Codend.Domain.ValueObjects.Primitives;

namespace Codend.Contracts.Automapper.ValueObjects;

public class NullableStringValueObjectProfile : Profile
{
    public NullableStringValueObjectProfile()
    {
        CreateMap<NullableStringValueObject, string?>().ConvertUsing(valueObject => valueObject.Value);
    }
}