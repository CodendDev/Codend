using AutoMapper;
using Codend.Contracts.Responses.Epic;
using Codend.Domain.Entities;

namespace Codend.Contracts.Automapper.Entities;

/// <summary>
/// <see cref="Epic"/> board automapper profile.
/// </summary>
public sealed class BoardEpicResponseProfile : Profile
{
    /// <summary>
    /// Initializes maps for <see cref="Epic"/> class.
    /// </summary>
    public BoardEpicResponseProfile()
    {
        CreateMap<Epic, BoardEpicResponse>();
    }
}