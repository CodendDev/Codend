using Codend.Application.ProjectTaskStatuses.Commands.CreateProjectTaskStatus;
using Codend.Application.ProjectTaskStatuses.Commands.DeleteProjectTaskStatus;
using Codend.Contracts;
using Codend.Contracts.Requests.ProjectTaskStatuses;
using Codend.Domain.Entities;
using Codend.Presentation.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

/// <summary>
/// Controller for <see cref="ProjectTaskStatus"/> commands.
/// </summary>
[Route("api/projects/{projectId:guid}/taskStatuses")]
public class ProjectTaskStatusController : ApiController
{
    /// <inheritdoc />
    public ProjectTaskStatusController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Creates task status with given properties.
    /// </summary>
    /// <param name="projectId">Id of the project to which task status will be assigned.</param>
    /// <param name="request">Request with name.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "name": "Story name"
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 200 on success
    /// - 400 with errors on failure
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromRoute] Guid projectId,
        [FromBody] CreateProjectTaskStatusRequest request)
    {
        var command = new CreateProjectTaskStatusCommand(request.Name, projectId);
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return BadRequest(response.MapToApiErrorsResponse());
    }

    /// <summary>
    /// Deletes project task status with given <paramref name="statusId"/>.
    /// </summary>
    /// <param name="projectId">Id of the project to which the task status belongs.</param>
    /// <param name="statusId">Id of the task status which will be deleted.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 404 on failure
    /// </returns>
    [HttpDelete("{statusId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid projectId,
        [FromRoute] Guid statusId)
    {
        var command = new DeleteProjectTaskStatusCommand(statusId);
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return NoContent();
        }

        return NotFound();
    }
}