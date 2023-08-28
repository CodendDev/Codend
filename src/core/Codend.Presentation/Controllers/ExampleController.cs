using Codend.Application.Core.Abstractions.Data;
using Codend.Presentation.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

public class ExampleController : ApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public ExampleController(IMediator mediator, IUnitOfWork unitOfWork) : base(mediator)
    {
        _unitOfWork = unitOfWork;
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
}