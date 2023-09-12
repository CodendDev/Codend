using AutoMapper;
using Codend.Contracts.Responses.ProjectTask;
using Codend.Domain.Entities;

namespace Codend.Contracts.Automapper.Entities.ProjectTask;

public sealed class AbstractProjectTaskResponseProfile : Profile
{
    public AbstractProjectTaskResponseProfile()
    {
        CreateMap<ProjectTaskBase, AbstractProjectTaskResponse>();
    }
}