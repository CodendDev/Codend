using Codend.Application.Projects.Commands.CreateProject;
using Codend.Presentation.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

[Route("project")]
public class ProjectController : ApiController
{
    public ProjectController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectCommand command)
    {
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            // TODO
            return Ok(response.Value.Id);
        }

        return BadRequest(response.Errors);
    }
}