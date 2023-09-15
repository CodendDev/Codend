using Codend.Application.Authentication.Login;
using Codend.Application.Authentication.Register;
using Codend.Contracts.Authentication;
using Codend.Presentation.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

/// <summary>
/// Controller containing endpoints associated with authentication.
/// </summary>
public class AuthenticationController : ApiController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
    /// </summary>
    public AuthenticationController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="command">The login command which includes the user email and password.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///         "email": "test@test.com",
    ///         "password: "password"
    ///     }
    /// </remarks>
    /// <returns>
    /// An HTTP response containing JWT token if the login was successful or an error response.
    /// </returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(400)]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var response = await Mediator.Send(command);
        if (response.IsFailed)
        {
            return BadRequest(response.Reasons);
        }

        return Ok(response.Value);
    }

    /// <summary>
    /// Registers new user and returns a JWT token.
    /// </summary>
    /// <param name="command">The register command which includes the user email, password, first name and last name.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///         "email": "test@test.com",
    ///         "password: "password",
    ///         "firstName": "Jan",
    ///         "lastName: "Kowalski"
    ///     }
    /// </remarks>
    /// <returns>
    /// An HTTP response containing JWT token if the registration was successful or an error response.
    /// </returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(400)]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var response = await Mediator.Send(command);
        if (response.IsFailed)
        {
            return BadRequest(response.Reasons);
        }

        return Ok(response.Value);
    }
}