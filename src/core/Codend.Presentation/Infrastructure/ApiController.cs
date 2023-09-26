using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Infrastructure;

/// <summary>
/// A base class for an Codend controller.
/// </summary>
[Route("api")]
public abstract class ApiController : ControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiController"/> class.
    /// </summary>
    protected ApiController(IMediator mediator) => Mediator = mediator;

    /// <summary>
    /// Mediator used for sending queries and commands.
    /// </summary>
    protected IMediator Mediator { get; }
}