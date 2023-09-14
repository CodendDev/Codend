using AutoMapper;
using Codend.Contracts.Responses.ProjectTask;
using Codend.Domain.Entities;

namespace Codend.Contracts.Automapper.Entities.ProjectTask;

/// <summary>
/// Automapper profile for <see cref="BaseProjectTaskResponse"/>.
/// </summary>
public sealed class BaseProjectTaskResponseProfile : Profile
{
    /// <summary>
    /// Implements all <see cref="BaseProjectTaskResponse"/> maps.
    /// </summary>
    public BaseProjectTaskResponseProfile()
    {
        CreateMap<BaseProjectTask, BaseProjectTaskResponse>();
    }
}