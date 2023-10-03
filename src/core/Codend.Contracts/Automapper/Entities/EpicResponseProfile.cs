using AutoMapper;
using Codend.Contracts.Responses.Epic;
using Codend.Domain.Entities;

namespace Codend.Contracts.Automapper.Entities;

/// <summary>
/// <see cref="Epic"/> automapper profile.
/// </summary>
public sealed class EpicResponseProfile : Profile
{
    /// <summary>
    /// Initializes maps for <see cref="Epic"/> class.
    /// </summary>
    public EpicResponseProfile()
    {
        CreateMap<Epic, EpicResponse>();
    }
}