using AutoMapper;
using Codend.Contracts.Responses.Backlog;
using Codend.Domain.Entities;

namespace Codend.Contracts.Automapper.Entities;

/// <summary>
/// <see cref="BacklogTaskResponse"/> automapper profile.
/// </summary>
public sealed class BacklogTaskResponseProfile : Profile
{
    /// <summary>
    /// Initializes maps for <see cref="BacklogTaskResponse"/> class.
    /// </summary>
    public BacklogTaskResponseProfile()
    {
        CreateMap<Epic, BacklogTaskResponseProfile>();
        CreateMap<Story, BacklogTaskResponseProfile>();
        CreateMap<BaseProjectTask, BacklogTaskResponseProfile>();
    }
}