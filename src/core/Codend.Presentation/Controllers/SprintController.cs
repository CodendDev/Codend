using Codend.Application.Sprints.Commands.AssignTasks;
using Codend.Application.Sprints.Commands.CreateSprint;
using Codend.Application.Sprints.Commands.DeleteSprint;
using Codend.Application.Sprints.Commands.MoveTask;
using Codend.Application.Sprints.Commands.RemoveTasks;
using Codend.Application.Sprints.Commands.UpdateSprint;
using Codend.Application.Sprints.Queries.GetActiveSprints;
using Codend.Application.Sprints.Queries.GetAssignableTasks;
using Codend.Application.Sprints.Queries.GetSprint;
using Codend.Application.Sprints.Queries.GetSprints;
using Codend.Contracts;
using Codend.Contracts.Requests;
using Codend.Contracts.Requests.Sprint;
using Codend.Contracts.Responses.Sprint;
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
    ///         "tasks": [
    ///             {
    ///                 "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///                 "type": "task"
    ///             },
    ///             {
    ///                 "id": "28b1b31e-7738-11ee-b962-0242ac120002",
    ///                 "type": "story"
    ///             },
    ///             {
    ///                 "id": "2d23f326-7738-11ee-b962-0242ac120002",
    ///                 "type": "epic"
    ///             }
    ///         ]
    ///     }
    /// 
    /// </remarks>
    /// <param name="projectId">Id of the project to which the sprint belongs.</param>
    /// <param name="sprintId">Id of the sprint to which tasks will be assigned.</param>
    /// <param name="request">Request with list of tasks ids.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 on failure
    /// - 404 on failure
    /// </returns>
    [HttpPost("{sprintId:guid}/tasks")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    ///         "tasks": [
    ///             {
    ///                 "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///                 "type": "base"
    ///             },
    ///             {
    ///                 "id": "28b1b31e-7738-11ee-b962-0242ac120002",
    ///                 "type": "story"
    ///             },
    ///             {
    ///                 "id": "2d23f326-7738-11ee-b962-0242ac120002",
    ///                 "type": "epic"
    ///             }
    ///         ]
    ///     }
    /// 
    /// </remarks>
    /// <param name="projectId">Id of the project to which the sprint belongs.</param>
    /// <param name="sprintId">Id of the sprint from which tasks will be removed.</param>
    /// <param name="request">Request with list of tasks ids.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 on failure
    /// - 404 on failure
    /// </returns>
    [HttpDelete("{sprintId:guid}/tasks")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Moves task position between two given lexorank positions.
    /// </summary>
    /// <param name="projectId">Id of the project task belongs to.</param>
    /// <param name="sprintId">Id of the sprint task belongs to.</param>
    /// <param name="request">Request containing prev and next lexorank values, and taskId with taskType.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "prev": "ie",
    ///         "next": "ik",
    ///         "taskRequest": {
    ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///             "type": "base"
    ///         }
    ///     }
    /// 
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 on failure
    /// - 404 on failure
    /// </returns>
    [HttpPost("{sprintId:guid}/tasks/move")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MoveTask(
        [FromRoute] Guid projectId,
        [FromRoute] Guid sprintId,
        [FromBody] MoveSprintTaskRequest request) =>
        await Resolver<MoveSprintTaskCommand>
            .IfRequestNotNull(request)
            .ResolverFor(new MoveSprintTaskCommand(
                sprintId.GuidConversion<SprintId>(),
                request.TaskRequest.MapToSprintTaskId(),
                request.Prev,
                request.Next))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Retrieves all project sprints and active sprint id.
    /// </summary>
    /// <param name="projectId">The id of the project.</param>
    /// <returns>
    /// HTTP response with status code:
    /// 200 - on success.
    /// 404 - when project was not found.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(SprintsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSprints([FromRoute] Guid projectId) =>
        await Resolver<GetSprintsQuery>
            .For(new GetSprintsQuery(projectId.GuidConversion<ProjectId>()))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Retrieves all active project sprints info.
    /// </summary>
    /// <param name="projectId">The id of the project.</param>
    /// <returns>
    /// HTTP response with status code:
    /// 200 - on success.
    /// 404 - when project was not found.
    /// </returns>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<SprintInfoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActiveSprintsInfo([FromRoute] Guid projectId) =>
        await Resolver<GetActiveSprintsQuery>
            .For(new GetActiveSprintsQuery(projectId.GuidConversion<ProjectId>()))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Retrieves all tasks that can be assigned to given sprint.
    /// </summary>
    /// <param name="projectId">The id of the project.</param>
    /// <param name="sprintId">The id of the sprint.</param>
    /// <returns>
    /// HTTP response with status code:
    /// 200 - on success.
    /// 404 - when sprint was not found.
    /// </returns>
    [HttpGet("{sprintId:guid}/assignable")]
    [ProducesResponseType(typeof(IEnumerable<SprintInfoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAssignableTasks([FromRoute] Guid projectId, [FromRoute] Guid sprintId) =>
        await Resolver<GetAssignableTasksQuery>
            .For(
                new GetAssignableTasksQuery
                (
                    projectId.GuidConversion<ProjectId>(),
                    sprintId.GuidConversion<SprintId>()
                )
            )
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Retrieves sprint details.
    /// </summary>
    /// <param name="projectId">The id of the project.</param>
    /// <param name="sprintId">The id of the sprint.</param>
    /// <returns>
    /// HTTP response with status code:
    /// 200 - on success.
    /// 404 - when sprint was not found.
    /// </returns>
    [HttpGet("{sprintId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<SprintInfoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSprintDetails([FromRoute] Guid projectId, [FromRoute] Guid sprintId) =>
        await Resolver<GetSprintQuery>
            .For(new GetSprintQuery(projectId.GuidConversion<ProjectId>(), sprintId.GuidConversion<SprintId>()))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();
}