using AutoMapper;
using Codend.Contracts.Responses.Story;
using Codend.Domain.Entities;

namespace Codend.Contracts.Automapper.Entities;

/// <summary>
/// <see cref="Story"/> automapper profile.
/// </summary>
public sealed class StoryResponseProfile : Profile
{
    /// <summary>
    /// Initializes maps for <see cref="Story"/> class.
    /// </summary>
    public StoryResponseProfile()
    {
        CreateMap<Story, StoryResponse>();
    }
}