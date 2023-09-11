using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask;
using Codend.Presentation.Requests.ProjectTasks.Create;
using Codend.Presentation.Requests.ProjectTasks.Update;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers.ProjectTask;

[Route("api/bugfix")]
public class BugfixProjectTaskController : ProjectTaskBaseController
{
    public BugfixProjectTaskController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBugfix(CreateBugfixProjectTaskRequest request)
        => await CreateTask<CreateBugfixProjectTaskRequest, CreateBugfixProjectTaskCommand>(request);

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBugfixTask(UpdateBugfixProjectTaskRequest request)
        => await UpdateTask<UpdateBugfixProjectTaskRequest, UpdateBugfixProjectTaskCommand>(request);
}