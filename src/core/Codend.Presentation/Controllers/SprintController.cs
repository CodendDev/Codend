using Codend.Application.Sprints.Commands.CreateSprint;
using Codend.Contracts;
using Codend.Contracts.Requests.Sprint;
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
/// Controller for <see cref="Sprint"/> commands.
/// </summary>
[Route("api/projects/{projectId:guid}/sprints")]
[Authorize(ProjectOperations.IsProjectMemberPolicy)]
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
}