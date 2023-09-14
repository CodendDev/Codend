using Codend.Application.Stories.Commands.CreateStory;
using Codend.Application.Stories.Commands.DeleteStory;
using Codend.Application.Stories.Commands.UpdateStory;
using Codend.Contracts;
using Codend.Domain.Core.Errors;
using Codend.Presentation.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

[Route("api/story")]
public class StoryController : ApiController
{
    public StoryController(IMediator mediator) : base(mediator)
    {
    }

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
    /// HTTP response with status code 204 on success.
    /// </returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        return BadRequest(response.Reasons);
    }
}