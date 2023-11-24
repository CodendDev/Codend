using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Application.Core.Abstractions.Services;
using Codend.Contracts.Responses;
using FluentResults;

namespace Codend.Application.Users.Queries.GetUserDetails;

/// <summary>
/// Query used for retrieving logged user details.
/// </summary>
public sealed record GetLoggedUserDetailsQuery()
    : IQuery<UserDetails>;

/// <summary>
/// <see cref="GetUserDetailsQueryHandler"/> handler.
/// </summary>
public class GetUserDetailsQueryHandler
    : IQueryHandler<GetLoggedUserDetailsQuery, UserDetails>
{
    private readonly IUserService _userService;
    private readonly IHttpContextProvider _contextProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserDetailsQueryHandler"/> class.
    /// </summary>
    public GetUserDetailsQueryHandler(
        IUserService userService, IHttpContextProvider contextProvider)
    {
        _userService = userService;
        _contextProvider = contextProvider;
    }

    /// <inheritdoc />
    public async Task<Result<UserDetails>> Handle(GetLoggedUserDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _contextProvider.UserId;
        var user = await _userService.GetUserByIdAsync(userId);
        return Result.Ok(user);
    }
}