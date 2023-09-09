using Codend.Application.ProjectTasks.Commands.AssignUser;
using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Application.ProjectTasks.Commands.DeleteProjectTask;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask;
using Codend.Application.ProjectTasks.Queries.GetProjectTaskById;
using Codend.Contracts.ProjectTasks;
using Codend.Presentation.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectTaskNotFound = Codend.Domain.Core.Errors.DomainErrors.ProjectTaskErrors.ProjectTaskNotFound;

namespace Codend.Presentation.Controllers;

[Route("api/task")]
public class ProjectTaskController : ApiController
{
    public ProjectTaskController(IMediator mediator) : base(mediator)
    {
    }

    [Route("bugfix")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBugfix(CreateBugfixRequest request)
    {
        var command = request.MapToCommand();

        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return BadRequest(response.Reasons);
    }

    [HttpPut]
    [Route("bugfix")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBugfixTask(BugfixUpdateProjectTaskRequest request)
    {
        var response = await Mediator.Send(request.MapToCommand());
        if (response.IsSuccess)
        {
            return NoContent();
        }

        if (response.HasError<ProjectTaskNotFound>())
        {
            return NotFound();
        }

        return BadRequest(response.Reasons);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBugfixTask(UpdateProjectTaskRequest request)
    {
        var response = await Mediator.Send(request.MapToCommand());
        if (response.IsSuccess)
        {
            return NoContent();
        }

        if (response.HasError<ProjectTaskNotFound>())
        {
            return NotFound();
        }

        return BadRequest(response.Reasons);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(DeleteProjectTaskCommand command)
    {
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return Ok();
        }

        return NotFound();
    }

    [Route("assign")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignUser(AssignUserCommand command)
    {
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return Ok();
        }

        return NotFound();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(GetProjectTaskByIdQuery query)
    {
        var response = await Mediator.Send(query);
        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return NotFound();
    }
}