using Codend.Application.ProjectTasks.Commands.AssignUser;
using Codend.Application.ProjectTasks.Commands.DeleteProjectTask;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask;
using Codend.Application.ProjectTasks.Queries.GetProjectTaskById;
using Codend.Domain.Entities;
using Codend.Presentation.Requests.ProjectTasks.Update;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers.ProjectTask;

[Route("api/task")]
public class AbstractProjectTaskController : ProjectTaskBaseController
{
    public AbstractProjectTaskController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAbstractTask(UpdateProjectTaskRequest request)
        => await UpdateTask<UpdateProjectTaskRequest, UpdateAbstractProjectTaskCommand>(request);

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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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