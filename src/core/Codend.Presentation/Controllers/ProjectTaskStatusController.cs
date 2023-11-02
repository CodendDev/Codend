using Codend.Application.ProjectTaskStatuses.Commands.CreateProjectTaskStatus;
using Codend.Application.ProjectTaskStatuses.Commands.DeleteProjectTaskStatus;
using Codend.Application.ProjectTaskStatuses.Commands.MoveProjectTaskStatus;
using Codend.Application.ProjectTaskStatuses.Commands.UpdateProjectTaskStatus;
using Codend.Application.ProjectTaskStatuses.Queries.GetProjectTaskStatuses;
using Codend.Contracts;
using Codend.Contracts.Requests.ProjectTaskStatuses;
using Codend.Contracts.Responses.ProjectTaskStatus;
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
/// Controller for <see cref="ProjectTaskStatus"/> commands.
/// </summary>
[Route("api/projects/{projectId:guid}/task-statuses")]
[Authorize(ProjectOperations.IsProjectMemberPolicy)]
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
    /// - 200 with created ProjectTaskStatusId on success
    /// - 400 with errors on failure
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromRoute] Guid projectId,
        [FromBody] CreateProjectTaskStatusRequest request) =>
        await Resolver<CreateProjectTaskStatusCommand>
            .IfRequestNotNull(request)
            .ResolverFor(new CreateProjectTaskStatusCommand(request.Name, projectId.GuidConversion<ProjectId>()))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

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
        [FromRoute] Guid statusId) =>
        await Resolver<DeleteProjectTaskStatusCommand>
            .For(new DeleteProjectTaskStatusCommand(statusId.GuidConversion<ProjectTaskStatusId>()))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Updates task status with id <paramref name="statusId"/>.
    /// </summary>
    /// <param name="projectId">Id of the project to which the task status belongs.</param>
    /// <param name="statusId">Id of the task status that will be updated.</param>
    /// <param name="request">Request the name.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "name": "New status name"
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 with errors on failure
    /// - 404 on failure
    /// </returns>
    [HttpPut("{statusId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid projectId,
        [FromRoute] Guid statusId,
        [FromBody] UpdateProjectTaskStatusRequest request) =>
        await Resolver<UpdateProjectTaskStatusCommand>
            .IfRequestNotNull(request)
            .ResolverFor(
                new UpdateProjectTaskStatusCommand(
                    statusId.GuidConversion<ProjectTaskStatusId>(),
                    request.Name
                )
            )
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Retrieves all task statuses of the project with id <paramref name="projectId"/>.
    /// </summary>
    /// <param name="projectId">Id of the project to which the task status belongs.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 200 on success with list of statuses
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProjectTaskStatusResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMany(
        [FromRoute] Guid projectId) =>
        await Resolver<GetProjectTaskStatusesQuery>
            .For(new GetProjectTaskStatusesQuery(projectId.GuidConversion<ProjectId>()))
            .Execute(query => Mediator.Send(query))
            .ResolveResponse();

    /// <summary>
    /// Moves status position between two given lexorank positions.
    /// </summary>
    /// <param name="projectId">Id of the project to which status belongs to.</param>
    /// <param name="statusId">Id of the status that will be moved.</param>
    /// <param name="request">Request containing prev and next lexorank values.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "prev": "ie",
    ///         "next": "ik"
    ///     }
    /// 
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 on failure
    /// - 404 on failure
    /// </returns>
    [HttpPost("{statusId:guid}/move")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MoveTask(
        [FromRoute] Guid projectId,
        [FromRoute] Guid statusId,
        [FromBody] MoveProjectTaskStatusRequest request) =>
        await Resolver<MoveProjectTaskStatusCommand>
            .IfRequestNotNull(request)
            .ResolverFor(
                new MoveProjectTaskStatusCommand(
                    projectId.GuidConversion<ProjectId>(),
                    statusId.GuidConversion<ProjectTaskStatusId>(),
                    request.Prev,
                    request.Next
                )
            )
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();
}