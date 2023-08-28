using Codend.Presentation.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

public class ExampleController : ApiController
{
    public ExampleController(IMediator mediator) : base(mediator)
    {
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