using Codend.Application.Projects.Commands.AddMember;
using Codend.Application.Projects.Commands.CreateProject;
using Codend.Application.Projects.Commands.DeleteProject;
using Codend.Application.Projects.Commands.RemoveMember;
using Codend.Application.Projects.Commands.UpdateProject;
using Codend.Application.Projects.Queries.GetMembers;
using Codend.Application.Projects.Queries.GetProjectById;
using Codend.Application.Projects.Queries.GetProjects;
using Codend.Contracts;
using Codend.Contracts.Common;
using Codend.Contracts.Requests.Project;
using Codend.Contracts.Responses;
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
[Route("api/projects")]
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
    ///         "description": "Not so long description"
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
    ///         "description": {
    ///             "shouldUpdate": true,
    ///             "value": "new description"
    ///         },
    ///         "defaultStatusId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    ///     }
    /// </remarks>
    /// <returns>
    /// A HTTP NoContent response if project was successfully updated or an error response.
    /// </returns>
    [HttpPut("{projectId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid projectId,
        [FromBody] UpdateProjectRequest request)
    {
        var command = new UpdateProjectCommand(
            projectId,
            request.Name,
            request.Description,
            request.DefaultStatusId);
        
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
    public async Task<IActionResult> Get([FromRoute] Guid projectId)
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
    /// Retrieves all matching projects with their common information.
    /// </summary>
    /// <param name="request">The get projects request which includes page size, page index, search text, sort column and sort order.</param>
    /// <returns>
    /// HTTP response with status code:
    /// 200 - on success with PagedList containing matching entities.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedList<ProjectResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllForUser([FromQuery] GetProjectsRequest request)
    {
        var query = new GetProjectsQuery(
            request.PageIndex,
            request.PageSize,
            request.SortColumn,
            request.SortOrder,
            request.Search);

        var response = await Mediator.Send(query);

        return Ok(response.Value);
    }

    /// <summary>
    /// Adds member with given <paramref name="userId"/> to project with given <paramref name="projectId"/>.
    /// </summary>
    /// <param name="projectId">The id of the project to which user will be added as member.</param>
    /// <param name="userId">The id of the user which will be added as member.</param>
    /// <returns>
    /// HTTP response with status code:
    /// 204 - on success
    /// 400 - on failure
    /// 404 - when user or project was not found
    /// </returns>
    [HttpPost("{projectId:guid}/members/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddMember(
        [FromRoute] Guid projectId,
        [FromRoute] Guid userId)
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
    /// <returns>
    /// HTTP response with status code:
    /// 204 - on success
    /// 404 - when user or project was not found
    /// </returns>
    [HttpDelete("{projectId:guid}/members/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveMember(
        [FromRoute] Guid projectId,
        [FromRoute] Guid userId)
    {
        var command = new RemoveMemberCommand(projectId.GuidConversion<ProjectId>(), userId.GuidConversion<UserId>());
        var response = await Mediator.Send(command);
        if (response.IsFailed)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Retrieves all project members who match given criteria.
    /// </summary>
    /// <param name="projectId">The id of the project whose members will be returned.</param>
    /// <param name="request">Get members request including search text.</param>
    /// <returns>
    /// HTTP response with status code:
    /// 200 - on success with users list.
    /// 404 - when project was not found.
    /// </returns>
    [HttpPost("{projectId:guid}/members")]
    [ProducesResponseType(typeof(IEnumerable<UserResponse>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMembers(
        [FromRoute] Guid projectId,
        [FromQuery] GetMembersRequest request)
    {
        var query = new GetMembersQuery(projectId.GuidConversion<ProjectId>(), request.Search);
        var response = await Mediator.Send(query);

        if (response.HasError<DomainNotFound>())
        {
            return NotFound();
        }

        return Ok(response.Value);
    }
}