using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.AssignUser;
using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Application.ProjectTasks.Commands.DeleteProjectTask;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Application.ProjectTasks.Queries.GetProjectTaskById;
using Codend.Presentation.Infrastructure;
using Codend.Presentation.Requests.ProjectTasks.Create;
using Codend.Presentation.Requests.ProjectTasks.Update;
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

    #region Create Task

    [Route("bugfix")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBugfix(CreateBugfixProjectTaskRequest request)
        => await CreateTask<CreateBugfixProjectTaskRequest, CreateBugfixProjectTaskCommand>(request);

    private async Task<IActionResult> CreateTask<TRequest, TCommand>(TRequest request)
        where TRequest : AbstractCreateProjectTaskRequest<TCommand>
        where TCommand : ICommand<Guid>, ICreateProjectTaskCommand
    {
        var command = request.MapToCommand();

        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return BadRequest(response.Reasons);
    }

    #endregion

    #region Update Task

    [HttpPut]
    [Route("bugfix")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBugfixTask(UpdateBugfixProjectTaskRequest request)
        => await UpdateTask<UpdateBugfixProjectTaskRequest, UpdateBugfixProjectTaskCommand>(request);

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAbstractTask(UpdateProjectTaskRequest request)
        => await UpdateTask<UpdateProjectTaskRequest, UpdateAbstractProjectTaskCommand>(request);

    private async Task<IActionResult> UpdateTask<TRequest, TCommand>(TRequest request)
        where TRequest : AbstractUpdateProjectTaskRequest<TCommand>
        where TCommand : ICommand, IUpdateProjectTaskCommand
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

    #endregion

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