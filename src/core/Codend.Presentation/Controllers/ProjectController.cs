using Codend.Application.Projects.Commands.CreateProject;
using Codend.Application.Projects.Commands.DeleteProject;
using Codend.Application.Projects.Commands.UpdateProject;
using Codend.Application.Projects.Queries.GetProjectById;
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

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteProjectCommand command)
    {
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateProjectCommand command)
    {
        var response = await Mediator.Send(command);
        if (response.IsFailed)
        {
            return Ok(response.Value.Id);
        }

        return BadRequest(response.Errors);
    }

    [HttpGet]
    public async Task<IActionResult> Get(GetProjectByIdQuery query)
    {
        var response = await Mediator.Send(query);
        if (response.IsFailed)
        {
            return NotFound();
        }

        return Ok(response.Value);
    }
}