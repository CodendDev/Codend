using Codend.Application.Projects.Commands.CreateProject;
using Codend.Application.Projects.Commands.DeleteProject;
using Codend.Application.Projects.Commands.UpdateProject;
using Codend.Application.Projects.Queries.GetProjectById;
using Codend.Contracts;
using Codend.Contracts.Responses.Project;
using Codend.Domain.Core.Errors;
using Codend.Presentation.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

[Route("api/project")]
public class ProjectController : ApiController
{
    public ProjectController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorsResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateProjectCommand command)
    {
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return CreatedAtAction(nameof(Get), new { id = response.Value }, response.Value);
        }

        return BadRequest(response.MapToApiErrorsResponse());
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateProjectCommand command)
    {
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return NoContent();
        }

        if (response.HasError<DomainErrors.ProjectErrors.ProjectNotFound>())
        {
            return NotFound();
        }

        return BadRequest(response.Reasons);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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