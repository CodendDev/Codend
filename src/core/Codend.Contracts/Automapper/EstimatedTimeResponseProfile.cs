using AutoMapper;
using Codend.Contracts.Responses.ProjectTask;

namespace Codend.Contracts.Automapper;

/// <summary>
/// <see cref="EstimatedTimeResponse"/> automapper profile.
/// </summary>
public sealed class EstimatedTimeResponseProfile : Profile
{
    /// <summary>
    /// Initializes maps for <see cref="EstimatedTimeResponse"/> class.
    /// </summary>
    public EstimatedTimeResponseProfile()
    {
        CreateMap<TimeSpan, EstimatedTimeResponse>()
            .ConstructUsing(span => new EstimatedTimeResponse(span.Minutes, span.Hours, span.Days));
    }
}