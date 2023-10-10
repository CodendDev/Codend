using AutoMapper;
using Codend.Application.Core.Abstractions.Authentication;
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
    private readonly IProjectMemberRepository _memberRepository;
    private readonly IHttpContextProvider _identityProvider;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetEpicByIdQueryHandler"/> class.
    /// </summary>
    public GetEpicByIdQueryHandler(
        IMapper mapper,
        IEpicRepository epicRepository,
        IProjectMemberRepository memberRepository,
        IHttpContextProvider identityProvider)
    {
        _mapper = mapper;
        _epicRepository = epicRepository;
        _memberRepository = memberRepository;
        _identityProvider = identityProvider;
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

        var userId = _identityProvider.UserId;
        if (await _memberRepository.IsProjectMember(userId, epic.ProjectId, cancellationToken) is false)
        {
            return DomainNotFound.Fail<Project>();
        }

        var dto = _mapper.Map<EpicResponse>(epic);
        return Result.Ok(dto);
    }
}