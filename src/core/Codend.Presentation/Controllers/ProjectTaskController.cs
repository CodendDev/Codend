using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.AssignUser;
using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;
using Codend.Application.ProjectTasks.Commands.DeleteProjectTask;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Application.ProjectTasks.Queries.GetProjectTaskById;
using Codend.Domain.Core.Errors;
using Codend.Presentation.Infrastructure;
using Codend.Presentation.Requests.Abstractions;
using Codend.Presentation.Requests.ProjectTasks.Create;
using Codend.Presentation.Requests.ProjectTasks.Update;
using Codend.Presentation.Requests.ProjectTasks.Update.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

[Route("api/task")]
public class ProjectTaskController : ApiController
{
    public ProjectTaskController(IMediator mediator) : base(mediator)
    {
    }

    #region Private methods

    private async Task<IActionResult> CreateTask<TRequest, TCommand>(TRequest request)
        where TRequest : IMapRequestToCommand<TCommand, Guid>
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

    private async Task<IActionResult> UpdateTask<TRequest, TCommand>(TRequest request)
        where TRequest : UpdateProjectTaskAbstractRequest<TCommand>
        where TCommand : ICommand, IUpdateProjectTaskCommand
    {
        var response = await Mediator.Send(request.MapToCommand());
        if (response.IsSuccess)
        {
            return NoContent();
        }

        if (response.HasError<DomainErrors.ProjectTaskErrors.ProjectTaskNotFound>())
        {
            return NotFound();
        }

        return BadRequest(response.Reasons);
    }

    #endregion

    #region Shared methods

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

    #endregion

    #region BaseProjectTask

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBaseTask(CreateBaseProjectTaskRequest request)
        => await CreateTask<CreateBaseProjectTaskRequest, CreateBaseProjectTaskCommand>(request);

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBaseTask(UpdateBaseProjectTaskRequest request)
        => await UpdateTask<UpdateBaseProjectTaskRequest, UpdateBaseProjectTaskCommand>(request);

    #endregion

    #region BugfixProjectTask

    [Route("bugfix")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBugfix(CreateBugfixProjectTaskRequest request)
        => await CreateTask<CreateBugfixProjectTaskRequest, CreateBugfixProjectTaskCommand>(request);

    [Route("bugfix")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBugfixTask(UpdateBugfixProjectTaskRequest request)
        => await UpdateTask<UpdateBugfixProjectTaskRequest, UpdateBugfixProjectTaskCommand>(request);

    #endregion
}