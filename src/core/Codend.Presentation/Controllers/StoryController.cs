using Codend.Application.Stories.Commands.CreateStory;
using Codend.Application.Stories.Commands.DeleteStory;
using Codend.Application.Stories.Commands.UpdateStory;
using Codend.Application.Stories.Queries.GetStoryById;
using Codend.Contracts;
using Codend.Contracts.Requests;
using Codend.Contracts.Requests.Story;
using Codend.Contracts.Responses.Story;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Presentation.Extensions;
using Codend.Presentation.Infrastructure;
using Codend.Presentation.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

/// <summary>
/// Controller for <see cref="Story"/> commands. 
/// </summary>
[Route("api/projects/{projectId:guid}/stories")]
[Authorize(ProjectOperations.IsProjectMemberPolicy)]
public class StoryController : ApiController
{
    /// <inheritdoc />
    public StoryController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Creates story with given properties.
    /// </summary>
    /// <param name="projectId">Id of the project where the story will be created.</param>
    /// <param name="request">Request with name, description, epicId and statusId.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "name": "Story name",
    ///         "description": "Story description",
    ///         "epicId": "bda4a1f5-e135-493c-852c-826e6f9fbcb0",
    ///         "statusId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 200 with created StoryId on success
    /// - 400 with errors on failure
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromRoute] Guid projectId,
        [FromBody] CreateStoryRequest request) =>
        await Resolver<CreateStoryCommand>
            .For(new CreateStoryCommand(
                projectId.GuidConversion<ProjectId>(),
                request.Name,
                request.Description,
                request.EpicId.GuidConversion<EpicId>(),
                request.StatusId.GuidConversion<ProjectTaskStatusId>()
            ))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Deletes story with given <paramref name="storyId"/>.
    /// </summary>
    /// <param name="projectId">Id of the project to which the story belongs.</param>
    /// <param name="storyId">Id of the story that will be deleted.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 404 on failure
    /// </returns>
    [HttpDelete("{storyId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid projectId,
        [FromRoute] Guid storyId) =>
        await Resolver<DeleteStoryCommand>
            .For(new DeleteStoryCommand(storyId.GuidConversion<StoryId>()))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Updates story with id <paramref name="storyId"/>.
    /// </summary>
    /// <param name="projectId">Id of the project to which the story belongs.</param>
    /// <param name="storyId">Id of the story that will be updated.</param>
    /// <param name="request">Request with id, name, description and statusId.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "name": "New story name",
    ///         "description": "New story description",
    ///         "epicId": {
    ///             "shouldUpdate": true,
    ///             "EpicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    ///         },
    ///         "statusId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 with errors on failure
    /// - 404 on failure
    /// </returns>
    [HttpPut("{storyId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid projectId,
        [FromRoute] Guid storyId,
        [FromBody] UpdateStoryRequest request) =>
        await Resolver<UpdateStoryCommand>
            .For(new UpdateStoryCommand(
                storyId.GuidConversion<StoryId>(),
                request.Name,
                request.Description,
                request.EpicId.HandleNull().Convert(EntityIdExtensions.GuidConversion<EpicId>),
                request.StatusId.GuidConversion<ProjectTaskStatusId>()
            ))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Retrieves common information about Story with given <paramref name="storyId"/>
    /// </summary>
    /// <param name="projectId">Id of the project to which the story belongs.</param>
    /// <param name="storyId">Id of the story that will be retrieved.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 200 on Success with StoryResponse
    /// - 404 on failure
    /// </returns>
    [HttpGet("{storyId:guid}")]
    [ProducesResponseType(typeof(StoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid projectId,
        [FromRoute] Guid storyId) =>
        await Resolver<GetStoryByIdQuery>
            .For(new GetStoryByIdQuery(storyId.GuidConversion<StoryId>()))
            .Execute(query => Mediator.Send(query))
            .ResolveResponse();
}