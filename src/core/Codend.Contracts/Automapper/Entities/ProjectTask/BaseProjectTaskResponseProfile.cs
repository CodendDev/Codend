using AutoMapper;
using Codend.Contracts.Responses.ProjectTask;
using Codend.Domain.Entities;

namespace Codend.Contracts.Automapper.Entities.ProjectTask;

/// <summary>
/// <see cref="BaseProjectTask"/> automapper profile.
/// </summary>
public sealed class BaseProjectTaskResponseProfile : Profile
{
    /// <summary>
    /// Initializes maps for <see cref="BaseProjectTaskResponse"/> class.
    /// </summary>
    public BaseProjectTaskResponseProfile()
    {
        CreateMap<BaseProjectTask, BaseProjectTaskResponse>();
    }
}