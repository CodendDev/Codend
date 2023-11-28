using Codend.Application.Users.Commands.UpdateUser;
using Codend.Application.Users.Queries.GetUserDetails;
using Codend.Contracts.Requests.User;
using Codend.Contracts.Responses;
using Codend.Presentation.Extensions;
using Codend.Presentation.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

/// <summary>
/// Controller containing endpoints associated with user management.
/// </summary>
[Route("api/user")]
public class UserController : ApiController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    public UserController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Retrieves logged user details.
    /// </summary>
    /// <returns>
    /// An HTTP response containing user details or an error.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(UserDetails), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLoggedUserDetails() =>
        await Resolver<GetLoggedUserDetailsQuery>
            .For(new GetLoggedUserDetailsQuery())
            .Execute(query => Mediator.Send(query))
            .ResolveResponse();

    /// <summary>
    /// Updates user data.
    /// </summary>
    /// <param name="request">The update request which includes the user email, first name and last name and imageUrl.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "firstName": "Jan",
    ///         "lastName": "Kowalski",
    ///         "imageUrl": "https://i.pravatar.cc/150?img=3"
    ///     }
    /// </remarks>
    /// <returns>
    /// An Ok HTTP response or a failure.
    /// </returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request) =>
        await Resolver<UpdateUserCommand>
            .IfRequestNotNull(request)
            .ResolverFor(new UpdateUserCommand(request.FirstName, request.LastName,
                request.ImageUrl))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();
}