﻿using Codend.Application.Authentication.Login;
using Codend.Application.Authentication.Register;
using Codend.Contracts;
using Codend.Contracts.Authentication;
using Codend.Presentation.Extensions;
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
    ///         "password": "password"
    ///     }
    /// </remarks>
    /// <returns>
    /// An HTTP response containing JWT token if the login was successful or an error response.
    /// </returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(typeof(ApiErrorsResponse), 400)]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command) =>
        await Resolver<LoginCommand>
            .IfRequestNotNull(command)
            .ResolverFor(command)
            .Execute(req => Mediator.Send(req))
            .ResolveResponse();


    /// <summary>
    /// Registers new user and returns a JWT token.
    /// </summary>
    /// <param name="command">The register command which includes the user email, password, first name and last name.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///         "email": "test@test.com",
    ///         "password": "password",
    ///         "firstName": "Jan",
    ///         "lastName": "Kowalski",
    ///         "imageUrl": "https://picsum.photos/400"
    ///     }
    /// </remarks>
    /// <returns>
    /// An HTTP response containing JWT token if the registration was successful or an error response.
    /// </returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(typeof(ApiErrorsResponse), 400)]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command) =>
        await Resolver<RegisterCommand>
            .IfRequestNotNull(command)
            .ResolverFor(command)
            .Execute(req => Mediator.Send(req))
            .ResolveResponse();
}