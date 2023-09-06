using AutoMapper;
using Codend.Domain.ValueObjects.Primitives;

namespace Codend.Contracts.Automapper.ValueObjects;

public class StringValueObjectProfile : Profile
{
    public StringValueObjectProfile()
    {
        CreateMap<StringValueObject, string>().ConvertUsing(valueObject => valueObject.Value);
    }
}