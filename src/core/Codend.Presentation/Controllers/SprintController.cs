using Codend.Application.Sprints.Commands.AssignTasks;
using Codend.Application.Sprints.Commands.CreateSprint;
using Codend.Application.Sprints.Commands.DeleteSprint;
using Codend.Application.Sprints.Commands.RemoveTasks;
using Codend.Application.Sprints.Commands.UpdateSprint;
using Codend.Contracts;
using Codend.Contracts.Requests;
using Codend.Contracts.Requests.Sprint;
using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Presentation.Extensions;
using Codend.Presentation.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Codend.Presentation.Infrastructure.Authorization.ProjectOperations;

namespace Codend.Presentation.Controllers;

/// <summary>
/// Controller for <see cref="Sprint"/> commands.
/// </summary>
[Route("api/projects/{projectId:guid}/sprints")]
[Authorize(IsProjectMemberPolicy)]
public class SprintController : ApiController
{
    /// <inheritdoc />
    public SprintController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Creates sprint with given properties.
    /// </summary>
    /// <param name="projectId">Id of the project where the story will be created.</param>
    /// <param name="request">Request with name, start date, end date and optional goal.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "name": "string",
    ///         "startDate": "2023-10-14T12:00:00.000Z",
    ///         "endDate": "2023-10-24T07:50:34.841Z",
    ///         "goal": "string"
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 200 with created SprintId on success
    /// - 400 with errors on failure
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromRoute] Guid projectId,
        [FromBody] CreateSprintRequest request) =>
        await Resolver<CreateSprintCommand>
            .IfRequestNotNull(request)
            .ResolverFor(new CreateSprintCommand(
                projectId.GuidConversion<ProjectId>(),
                request.Name,
                request.StartDate,
                request.EndDate,
                request.Goal
            ))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Updates sprint with id <paramref name="sprintId"/>.
    /// </summary>
    /// <param name="projectId">Id of the project to which the sprint belongs.</param>
    /// <param name="sprintId">Id of the sprint that will be updated.</param>
    /// <param name="request">Request with name, start date, end date and goal.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///         "name": "string",
    ///         "startDate": "2023-9-24T08:24:03.557Z",
    ///         "endDate": "2023-10-24T08:24:03.557Z",
    ///         "goal": {
    ///         "shouldUpdate": true,
    ///         "value": "string"
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 with errors on failure
    /// - 404 on failure
    /// </returns>
    [HttpPut("{sprintId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid projectId,
        [FromRoute] Guid sprintId,
        [FromBody] UpdateSprintRequest request) =>
        await Resolver<UpdateSprintCommand>
            .IfRequestNotNull(request)
            .ResolverFor(new UpdateSprintCommand(
                sprintId.GuidConversion<SprintId>(),
                request.Name,
                request.StartDate,
                request.EndDate,
                request.Goal.HandleNull()
            ))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Deletes sprint with given <paramref name="sprintId"/>.
    /// </summary>
    /// <param name="projectId">Id of the project to which the sprint belongs.</param>
    /// <param name="sprintId">Id of the sprint that will be deleted.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 404 on failure
    /// </returns>
    [HttpDelete("{sprintId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid projectId,
        [FromRoute] Guid sprintId) =>
        await Resolver<DeleteSprintCommand>
            .For(new DeleteSprintCommand(sprintId.GuidConversion<SprintId>()))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Assigns tasks to sprint with given <paramref name="sprintId"/>
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///         "tasksIds": [
    ///             "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///             "758ab11c-74b9-11ee-b962-0242ac120002"
    ///         ]
    ///     }
    /// </remarks>
    /// <param name="projectId">Id of the project to which the sprint belongs.</param>
    /// <param name="sprintId">Id of the sprint to which tasks will be assigned.</param>
    /// <param name="request">Request with list of tasks ids.</param>
    /// <returns></returns>
    [HttpPost("{sprintId:guid}/tasks")]
    public async Task<IActionResult> AssignTasks(
        [FromRoute] Guid projectId,
        [FromRoute] Guid sprintId,
        [FromBody] SprintTasksRequest request) =>
        await Resolver<SprintAssignTasksCommand>
            .IfRequestNotNull(request)
            .ResolverFor(
                new SprintAssignTasksCommand
                (
                    sprintId.GuidConversion<SprintId>(),
                    request.Tasks.Select<SprintTaskRequest, ISprintTaskId>(task => task.MapToSprintTaskId())
                )
            )
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Removes tasks to sprint with given <paramref name="sprintId"/>
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///         "tasksIds": [
    ///             "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///             "758ab11c-74b9-11ee-b962-0242ac120002"
    ///         ]
    ///     }
    /// </remarks>
    /// <param name="projectId">Id of the project to which the sprint belongs.</param>
    /// <param name="sprintId">Id of the sprint from which tasks will be removed.</param>
    /// <param name="request">Request with list of tasks ids.</param>
    /// <returns></returns>
    [HttpDelete("{sprintId:guid}/tasks")]
    public async Task<IActionResult> RemoveTasks(
        [FromRoute] Guid projectId,
        [FromRoute] Guid sprintId,
        [FromBody] SprintTasksRequest request) =>
        await Resolver<SprintRemoveTasksCommand>
            .IfRequestNotNull(request)
            .ResolverFor(
                new SprintRemoveTasksCommand
                (
                    sprintId.GuidConversion<SprintId>(),
                    request.Tasks.Select<SprintTaskRequest, ISprintTaskId>(task => task.MapToSprintTaskId())
                )
            )
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();
}