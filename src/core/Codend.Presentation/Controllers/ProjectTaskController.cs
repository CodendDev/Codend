using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Contracts.ProjectTasks;
using Codend.Domain.Core.Enums;
using Codend.Domain.Entities;
using Codend.Presentation.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

[Route("api/task")]
public class ProjectTaskController : ApiController
{
    public ProjectTaskController(IMediator mediator) : base(mediator)
    {
    }

    [Route("bugfix")]
    [HttpPost]
    public async Task<IActionResult> CreateBugFix(CreateBugfixRequest request)
    {
        // TODO clean?
        var command = new CreateBugfixProjectTaskCommand(
            new BugFixProjectTaskProperties(
                request.Name,
                ProjectTaskPriority.FromName(request.Priority),
                new ProjectTaskStatusId(request.Status),
                new ProjectId(request.ProjectId),
                request.Description,
                request.EstimatedTime,
                request.DueDate,
                request.StoryPoints,
                request.AssigneeId is not null ? new UserId(request.AssigneeId.Value) : null
            ));

        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return BadRequest(response.Errors);
    }
}