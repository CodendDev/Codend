using AutoMapper;
using Codend.Contracts.Responses.Story;
using Codend.Domain.Entities;

namespace Codend.Contracts.Automapper.Entities;

/// <summary>
/// <see cref="Story"/> board automapper profile.
/// </summary>
public sealed class BoardStoryResponseProfile : Profile
{
    /// <summary>
    /// Initializes maps for <see cref="Story"/> class.
    /// </summary>
    public BoardStoryResponseProfile()
    {
        CreateMap<Story, BoardStoryResponse>();
    }
}