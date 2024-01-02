using AutoMapper;
using Codend.Shared.Infrastructure.Lexorank;

namespace Codend.Contracts.Automapper;

/// <summary>
/// <see cref="Lexorank"/> automapper profile.
/// </summary>
public class LexorankProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LexorankProfile"/> class.
    /// </summary>
    public LexorankProfile()
    {
        CreateMap<Lexorank, string>().ConvertUsing(lexorank => lexorank.Value);
        CreateMap<Lexorank?, string?>().ConvertUsing(lexorank => lexorank != null ? lexorank.Value : null);
    }
}