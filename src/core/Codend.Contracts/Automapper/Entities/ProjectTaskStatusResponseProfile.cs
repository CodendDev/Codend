using AutoMapper;
using Codend.Contracts.Responses.ProjectTaskStatus;
using Codend.Domain.Entities;

namespace Codend.Contracts.Automapper.Entities;

/// <summary>
/// <see cref="ProjectTaskStatus"/> automapper profile.
/// </summary>
public sealed class ProjectTaskStatusResponseProfile : Profile
{
    /// <summary>
    /// Initializes maps for <see cref="ProjectTaskStatus"/> class.
    /// </summary>
    public ProjectTaskStatusResponseProfile()
    {
        CreateMap<ProjectTaskStatus, ProjectTaskStatusResponse>();
    }
}