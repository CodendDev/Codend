using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Infrastructure;

[Route("api")]
public class ApiController : ControllerBase
{
    protected ApiController(IMediator mediator) => Mediator = mediator;
    
    protected IMediator Mediator { get; }
}