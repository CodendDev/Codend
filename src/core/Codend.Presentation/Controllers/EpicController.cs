using Codend.Application.Epics.Commands.CreateEpic;
using Codend.Application.Epics.Commands.DeleteEpic;
using Codend.Application.Epics.Commands.UpdateEpic;
using Codend.Contracts;
using Codend.Contracts.Requests.Epic;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Presentation.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

/// <summary>
/// Controller for <see cref="Epic"/> commands.
/// </summary>
[Route("api/epic")]
public class EpicController : ApiController
{
    /// <inheritdoc />
    public EpicController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Creates epic with given properties.
    /// </summary>
    /// <param name="request">Request with name, description and project Id.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "name": "Epic name",
    ///         "description: "Epic description",
    ///         "projectId: "bda4a1f5-e135-493c-852c-826e6f9fbcb0",
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 with errors on failure
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEpicRequest request)
    {
        var command = new CreateEpicCommand(request.Name, request.Description, request.ProjectId);
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return BadRequest(response.MapToApiErrorsResponse());
    }

    /// <summary>
    /// Deletes epic with given <paramref name="epicId"/>.
    /// </summary>
    /// <param name="epicId">Id of the epic which will be deleted.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 404 on failure
    /// </returns>
    [HttpDelete("{epicId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid epicId)
    {
        var command = new DeleteEpicCommand(epicId);
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return NoContent();
        }

        return NotFound();
    }

    /// <summary>
    /// Updates epic with id <paramref name="epicId"/>.
    /// </summary>
    /// <param name="epicId">Id of the epic that will be updated.</param>
    /// <param name="request">Request name and description.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "name": "New epic name",
    ///         "description: "New epic description"
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 with errors on failure
    /// - 404 on failure
    /// </returns>
    [HttpPut("{epicId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid epicId, [FromBody] UpdateEpicRequest request)
    {
        var command = new UpdateEpicCommand(epicId, request.Name, request.Description);
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return NoContent();
        }

        if (response.HasError<DomainErrors.General.DomainNotFound>())
        {
            return NotFound();
        }

        return BadRequest(response.MapToApiErrorsResponse());
    }
}