using Codend.Application.Projects.Commands.AddMember;
using Codend.Application.Projects.Commands.CreateProject;
using Codend.Application.Projects.Commands.DeleteProject;
using Codend.Application.Projects.Commands.RemoveMember;
using Codend.Application.Projects.Commands.UpdateProject;
using Codend.Application.Projects.Queries.GetProjectById;
using Codend.Contracts;
using Codend.Contracts.Requests.Project;
using Codend.Contracts.Responses.Project;
using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Presentation.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Presentation.Controllers;

/// <summary>
/// Controller containing endpoints associated with <see cref="Project"/> entity management.
/// </summary>
[Route("api/project")]
public class ProjectController : ApiController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectController"/> class.
    /// </summary>
    public ProjectController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Creates new Project entity.
    /// </summary>
    /// <param name="request">The create project request which includes project name and description.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "name": "New project name",
    ///         "description: "Not so long description"
    ///     }
    /// </remarks>
    /// <returns>
    /// An HTTP response containing newly created project id if action was successful or an error response.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProjectRequest request)
    {
        var command = new CreateProjectCommand(request.Name, request.Description);
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return BadRequest(response.MapToApiErrorsResponse());
    }

    /// <summary>
    /// Deletes Project entity with given <paramref name="projectId"/>.
    /// </summary>
    /// <param name="projectId">Id of the project that will be deleted.</param>
    /// <returns>
    /// A HTTP NoContent response if project was successfully deleted or an error response.
    /// </returns>
    [HttpDelete("{projectId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid projectId)
    {
        var command = new DeleteProjectCommand(projectId);
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return NoContent();
        }

        return NotFound();
    }

    /// <summary>
    /// Updated the Project entity with given <paramref name="projectId"/>.
    /// </summary>
    /// <param name="projectId">Id of the project that will be updated.</param>
    /// <param name="request">The update project request which includes project name and description.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///         "name": "Updated project name",
    ///         "description: "Updated project description"
    ///     }
    /// </remarks>
    /// <returns>
    /// A HTTP NoContent response if project was successfully updated or an error response.
    /// </returns>
    [HttpPut("{projectId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid projectId, [FromBody] UpdateProjectRequest request)
    {
        var command = new UpdateProjectCommand(projectId, request.Name, request.Description);
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return NoContent();
        }

        if (response.HasError<DomainNotFound>())
        {
            return NotFound();
        }

        return BadRequest(response.MapToApiErrorsResponse());
    }

    /// <summary>
    /// Retrieves common information about Project with given <paramref name="projectId"/>
    /// </summary>
    /// <param name="projectId">The id of the project which data will be returned.</param>
    /// <returns>
    /// A HTTP OK response with project data if query was successful or an error response.
    /// </returns>
    [HttpGet("{projectId:guid}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid projectId)
    {
        var query = new GetProjectByIdQuery(projectId);
        var response = await Mediator.Send(query);
        if (response.IsFailed)
        {
            return NotFound();
        }

        return Ok(response.Value);
    }

    /// <summary>
    /// Adds member with given <paramref name="userId"/> to project with given <paramref name="projectId"/>.
    /// </summary>
    /// <param name="projectId">The id of the project to which user will be added as member.</param>
    /// <param name="userId">The id of the user which will be added as member.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///         "projectId": "1f0c1930-50f4-4f17-8470-211b3a5cc873",
    ///         "userId: "e405f337-4da0-4cce-818b-9231642c93fe"
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// 204 - on success
    /// 400 - on failure
    /// 404 - when user or project was not found
    /// </returns>
    [HttpPost("{projectId:guid}/member/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddMember(Guid projectId, Guid userId)
    {
        var command = new AddMemberCommand(projectId.GuidConversion<ProjectId>(), userId.GuidConversion<UserId>());
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return NoContent();
        }

        if (response.HasError<DomainErrors.ProjectMember.UserIsProjectMemberAlready>())
        {
            return BadRequest(response.MapToApiErrorsResponse());
        }

        return NotFound();
    }

    /// <summary>
    /// Removes member with given <paramref name="userId"/> from project with given <paramref name="projectId"/>.
    /// </summary>
    /// <param name="projectId">The id of the project from which user will be removed.</param>
    /// <param name="userId">The id of the user which will be removed.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///         "projectId": "1f0c1930-50f4-4f17-8470-211b3a5cc873",
    ///         "userId: "e405f337-4da0-4cce-818b-9231642c93fe"
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// 204 - on success
    /// 404 - when user or project was not found
    /// </returns>
    [HttpDelete("{projectId:guid}/member/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveMember(Guid projectId, Guid userId)
    {
        var command = new RemoveMemberCommand(projectId.GuidConversion<ProjectId>(), userId.GuidConversion<UserId>());
        var response = await Mediator.Send(command);
        if (response.IsFailed)
        {
            return NotFound();
        }

        return NoContent();
    }
}