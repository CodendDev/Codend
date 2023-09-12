using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Domain.Core.Errors;
using Codend.Presentation.Infrastructure;
using Codend.Presentation.Requests.ProjectTasks.Create;
using Codend.Presentation.Requests.ProjectTasks.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers.ProjectTask;

public abstract class ProjectTaskBaseController : ApiController
{
    protected ProjectTaskBaseController(IMediator mediator) : base(mediator)
    {
    }

    protected async Task<IActionResult> CreateTask<TRequest, TCommand>(TRequest request)
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

    protected async Task<IActionResult> UpdateTask<TRequest, TCommand>(TRequest request)
        where TRequest : AbstractUpdateProjectTaskRequest<TCommand>
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
}