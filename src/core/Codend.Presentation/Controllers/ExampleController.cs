using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Presentation.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

public class ExampleController : ApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public ExampleController(IMediator mediator, IUnitOfWork unitOfWork, IAuthService authService) : base(mediator)
    {
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    [HttpGet("test-ok")]
    public IActionResult TestOk()
    {
        return Ok();
    }

    [HttpGet("test-bad-request")]
    public IActionResult TestBadRequest()
    {
        return BadRequest();
    }
    
    [HttpGet("test-login")]
    public async Task<IActionResult> TestLogin(string email, string password)
    {
        var response = await _authService.LoginAsync(email, password);
        return Ok(response);
    }
    
    [Authorize]
    [HttpGet("test-authentication")]
    public IActionResult TestAuthentication()
    {
        return Ok();
    }
}