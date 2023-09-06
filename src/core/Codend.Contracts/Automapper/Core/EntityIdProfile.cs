using AutoMapper;
using Codend.Domain.Core.Abstractions;

namespace Codend.Contracts.Automapper.Core;

public class EntityIdProfile : Profile
{
    public EntityIdProfile()
    {
        CreateMap<IEntityId<Guid>, Guid>().ConvertUsing(strongId => strongId.Value);
    }
}