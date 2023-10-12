using AutoMapper;
using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Contracts.Responses.Story;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Stories.Queries.GetStoryById;

/// <summary>
/// Command to retrieve story by it's id.
/// </summary>
/// <param name="Id">Id of the story to be retrieved.</param>
public sealed record GetStoryByIdQuery(
        StoryId Id)
    : IQuery<StoryResponse>;

/// <summary>
/// <see cref="GetStoryByIdQuery"/> Handler.
/// </summary>
public class GetStoryByIdQueryHandler : IQueryHandler<GetStoryByIdQuery, StoryResponse>
{
    private readonly IStoryRepository _storyRepository;
    private readonly IProjectMemberRepository _memberRepository;
    private readonly IHttpContextProvider _contextProvider;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetStoryByIdQueryHandler"/> class.
    /// </summary>
    public GetStoryByIdQueryHandler(
        IStoryRepository storyRepository,
        IProjectMemberRepository memberRepository,
        IHttpContextProvider contextProvider,
        IMapper mapper)
    {
        _storyRepository = storyRepository;
        _memberRepository = memberRepository;
        _contextProvider = contextProvider;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Result<StoryResponse>> Handle(GetStoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var story = await _storyRepository.GetByIdAsync(request.Id, cancellationToken);
        if (story is null)
        {
            return DomainNotFound.Fail<Story>();
        }

        var userId = _contextProvider.UserId;
        if (await _memberRepository.IsProjectMember(userId, story.ProjectId, cancellationToken) is false)
        {
            return DomainNotFound.Fail<Project>();
        }

        var dto = _mapper.Map<StoryResponse>(story);
        return Result.Ok(dto);
    }
}