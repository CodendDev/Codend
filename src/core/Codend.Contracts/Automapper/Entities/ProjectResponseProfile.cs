using AutoMapper;
using Codend.Contracts.Responses.Project;
using Codend.Domain.Entities;

namespace Codend.Contracts.Automapper.Entities;

/// <summary>
/// <see cref="Project"/> automapper profile.
/// </summary>
public sealed class ProjectResponseProfile : Profile
{
    /// <summary>
    /// Initializes maps for <see cref="Project"/> class.
    /// </summary>
    public ProjectResponseProfile()
    {
        CreateMap<Project, ProjectResponse>();
    }
}