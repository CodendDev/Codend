using AutoMapper;
using Codend.Contracts.Responses.ProjectTask;

namespace Codend.Contracts.Automapper;

public class EstimatedTimeResponseProfile : Profile
{
    public EstimatedTimeResponseProfile()
    {
        CreateMap<TimeSpan, EstimatedTimeResponse>()
            .ConstructUsing(span => new EstimatedTimeResponse(span.Minutes, span.Hours, span.Days));
    }
}