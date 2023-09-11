using AutoMapper;
using Codend.Contracts.Responses.ProjectTask;
using Codend.Domain.Entities;

namespace Codend.Contracts.Automapper.Entities;

public sealed class AbstractProjectTaskResponseProfile : Profile
{
    public AbstractProjectTaskResponseProfile()
    {
        CreateMap<AbstractProjectTask, AbstractProjectTaskResponse>();
    }
}