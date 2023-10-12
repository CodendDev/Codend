using AutoMapper;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Contracts.Responses.Epic;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Epics.Queries.GetEpicById;

/// <summary>
/// Command to retrieve epic by it's id.
/// </summary>
/// <param name="Id">Id of the epic to be retrieved.</param>
public sealed record GetEpicByIdQuery(
        EpicId Id)
    : IQuery<EpicResponse>;

/// <summary>
/// <see cref="GetEpicByIdQuery"/> Handler.
/// </summary>
public class GetEpicByIdQueryHandler : IQueryHandler<GetEpicByIdQuery, EpicResponse>
{
    private readonly IEpicRepository _epicRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetEpicByIdQueryHandler"/> class.
    /// </summary>
    public GetEpicByIdQueryHandler(
        IEpicRepository epicRepository,
        IMapper mapper)
    {
        _epicRepository = epicRepository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Result<EpicResponse>> Handle(GetEpicByIdQuery request,
        CancellationToken cancellationToken)
    {
        var epic = await _epicRepository.GetByIdAsync(request.Id, cancellationToken);
        if (epic is null)
        {
            return DomainNotFound.Fail<Epic>();
        }

        var dto = _mapper.Map<EpicResponse>(epic);
        return Result.Ok(dto);
    }
}