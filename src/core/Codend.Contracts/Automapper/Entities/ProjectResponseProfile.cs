using AutoMapper;
using Codend.Contracts.Responses.Project;

namespace Codend.Contracts.Automapper.Entities;

public sealed class ProjectResponseProfile : Profile
{
    public ProjectResponseProfile()
    {
        CreateMap<Domain.Entities.Project, ProjectResponse>();
    }
}