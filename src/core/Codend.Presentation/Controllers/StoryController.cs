using Codend.Application.Stories.Commands.CreateStory;
using Codend.Application.Stories.Commands.DeleteStory;
using Codend.Application.Stories.Commands.UpdateStory;
using Codend.Contracts;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Presentation.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

/// <summary>
/// Controller for <see cref="Story"/> commands. 
/// </summary>
[Route("api/story")]
public class StoryController : ApiController
{
    /// <inheritdoc />
    public StoryController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Creates story with given properties.
    /// </summary>
    /// <param name="command">Command with name and description.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 with errors on failure
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateStoryCommand command)
    {
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(response.MapToApiErrorsResponse());
    }

    /// <summary>
    /// Deletes story with given id.
    /// </summary>
    /// <param name="command">Command with id.</param>
    /// <returns>HTTP response with status code 204 on success.</returns>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(DeleteStoryCommand command)
    {
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return Ok();
        }

        return NotFound();
    }

    /// <summary>
    /// Updates story with given properties.
    /// </summary>
    /// <param name="command">Command with id, name and description.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 with errors on failure
    /// </returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateStoryCommand command)
    {
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return NoContent();
        }

        if (response.HasError<DomainErrors.StoryErrors.StoryNotFound>())
        {
            return NotFound();
        }

        return BadRequest(response.MapToApiErrorsResponse());
    }
}