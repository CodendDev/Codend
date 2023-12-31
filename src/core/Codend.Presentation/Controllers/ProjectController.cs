using Codend.Application.Projects.Commands.AddMember;
using Codend.Application.Projects.Commands.CreateProject;
using Codend.Application.Projects.Commands.DeleteProject;
using Codend.Application.Projects.Commands.DisableNotifications;
using Codend.Application.Projects.Commands.EnableNotifications;
using Codend.Application.Projects.Commands.RemoveMember;
using Codend.Application.Projects.Commands.UpdateIsFavouriteFlag;
using Codend.Application.Projects.Commands.UpdateProject;
using Codend.Application.Projects.Queries.GetBacklog;
using Codend.Application.Projects.Queries.GetBoard;
using Codend.Application.Projects.Queries.GetMembers;
using Codend.Application.Projects.Queries.GetProjectById;
using Codend.Application.Projects.Queries.GetProjects;
using Codend.Contracts;
using Codend.Contracts.Common;
using Codend.Contracts.Requests;
using Codend.Contracts.Requests.Project;
using Codend.Contracts.Responses;
using Codend.Contracts.Responses.Backlog;
using Codend.Contracts.Responses.Board;
using Codend.Contracts.Responses.Project;
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
    public async Task<IActionResult> Create([FromBody] CreateProjectRequest request) =>
        await Resolver<CreateProjectCommand>
            .IfRequestNotNull(request)
            .ResolverFor(new CreateProjectCommand(request.Name, request.Description))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

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
    [Authorize(IsProjectOwnerPolicy)]
    public async Task<IActionResult> Delete([FromRoute] Guid projectId) =>
        await Resolver<DeleteProjectCommand>
            .For(new DeleteProjectCommand(projectId.GuidConversion<ProjectId>()))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

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
    [Authorize(IsProjectMemberPolicy)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid projectId,
        [FromBody] UpdateProjectRequest request) =>
        await Resolver<UpdateProjectCommand>
            .IfRequestNotNull(request)
            .ResolverFor(new UpdateProjectCommand(
                projectId.GuidConversion<ProjectId>(),
                request.Name,
                request.Description.HandleNull(),
                request.DefaultStatusId.GuidConversion<ProjectTaskStatusId>()
            ))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

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
    [Authorize(IsProjectMemberPolicy)]
    public async Task<IActionResult> Get([FromRoute] Guid projectId) =>
        await Resolver<GetProjectByIdQuery>
            .For(new GetProjectByIdQuery(projectId.GuidConversion<ProjectId>()))
            .Execute(query => Mediator.Send(query))
            .ResolveResponse();

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
    public async Task<IActionResult> GetAllForUser([FromQuery] GetProjectsRequest request) =>
        await Resolver<GetProjectsQuery>
            .IfRequestNotNull(request)
            .ResolverFor(new GetProjectsQuery(
                request.PageIndex,
                request.PageSize,
                request.SortColumn,
                request.SortOrder,
                request.Search
            ))
            .Execute(query => Mediator.Send(query))
            .ResolveResponse();

    /// <summary>
    /// Adds member with given <paramref name="email"/> to project with given <paramref name="projectId"/>.
    /// </summary>
    /// <param name="projectId">The id of the project to which user will be added as member.</param>
    /// <param name="email">The email of the site user.</param>
    /// <returns>
    /// HTTP response with status code:
    /// 204 - on success
    /// 400 - on failure
    /// 404 - when user or project was not found
    /// </returns>
    [HttpPost("{projectId:guid}/members")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(IsProjectMemberPolicy)]
    public async Task<IActionResult> AddMember(
        [FromRoute] Guid projectId,
        [FromQuery] string email) =>
        await Resolver<AddMemberCommand>
            .For(new AddMemberCommand(projectId.GuidConversion<ProjectId>(), email))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

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
    [Authorize(IsProjectMemberPolicy)]
    public async Task<IActionResult> RemoveMember(
        [FromRoute] Guid projectId,
        [FromRoute] Guid userId) =>
        await Resolver<RemoveMemberCommand>
            .For(new RemoveMemberCommand(projectId.GuidConversion<ProjectId>(), userId.GuidConversion<UserId>()))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

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
    [HttpGet("{projectId:guid}/members")]
    [ProducesResponseType(typeof(IEnumerable<UserDetails>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IsProjectMemberPolicy)]
    public async Task<IActionResult> GetMembers(
        [FromRoute] Guid projectId,
        [FromQuery] GetMembersRequest request) =>
        await Resolver<GetMembersQuery>
            .IfRequestNotNull(request)
            .ResolverFor(new GetMembersQuery(projectId.GuidConversion<ProjectId>(), request.Search))
            .Execute(query => Mediator.Send(query))
            .ResolveResponse();

    /// <summary>
    /// Retrieves all project tasks, stories and epics within one object.
    /// </summary>
    /// <param name="projectId">The id of the project which elements will be returned.</param>
    /// <param name="sprintId">The id of the sprint which elements will be returned.</param>
    /// <param name="assigneeId">The id of the user whose elements will be returned</param>
    /// <returns>
    /// HTTP response with status code:
    /// 200 - on success with board response.
    /// 404 - when project was not found.
    /// </returns>
    [HttpGet("{projectId:guid}/board/{sprintId:guid}")]
    [ProducesResponseType(typeof(BoardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IsProjectMemberPolicy)]
    public async Task<IActionResult> GetBoard(
        [FromRoute] Guid projectId,
        [FromRoute] Guid sprintId,
        [FromQuery] Guid? assigneeId
    ) =>
        await Resolver<GetBoardQuery>
            .For(
                new GetBoardQuery(
                    projectId.GuidConversion<ProjectId>(),
                    sprintId.GuidConversion<SprintId>(),
                    assigneeId.GuidConversion<UserId>()
                )
            )
            .Execute(query => Mediator.Send(query))
            .ResolveResponse();

    /// <summary>
    /// Retrieves all project tasks, stories and epics within one sorted (onCreated criteria) list as compact tasks.
    /// </summary>
    /// <param name="projectId">The id of the project whose elements will be returned.</param>
    /// <returns>
    /// HTTP response with status code:
    /// 200 - on success with backlog response.
    /// 404 - when project was not found.
    /// </returns>
    [HttpGet("{projectId:guid}/backlog")]
    [ProducesResponseType(typeof(BacklogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IsProjectMemberPolicy)]
    public async Task<IActionResult> GetBacklog(
        [FromRoute] Guid projectId) =>
        await Resolver<GetBacklogQuery>
            .For(new GetBacklogQuery(projectId.GuidConversion<ProjectId>()))
            .Execute(query => Mediator.Send(query))
            .ResolveResponse();

    /// <summary>
    /// Updates IsFavourite flag for user for chosen project.
    /// </summary>
    /// <param name="projectId">The id of the project which IsFavourite flag will be updated.</param>
    /// <param name="request">Request containing new IsFavourite flag value.</param>
    /// <returns>
    /// HTTP response with status code:
    /// 204 - on success.
    /// 400 - on failure.
    /// 404 - when project was not found.
    /// </returns>
    [HttpPut("{projectId:guid}/favourite")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IsProjectMemberPolicy)]
    public async Task<IActionResult> UpdateProjectIsFavouriteFlag(
        [FromRoute] Guid projectId,
        [FromBody] UpdateProjectIsFavouriteFlagRequest request) =>
        await Resolver<UpdateProjectIsFavouriteFlagCommand>
            .IfRequestNotNull(request)
            .ResolverFor(new UpdateProjectIsFavouriteFlagCommand(
                projectId.GuidConversion<ProjectId>(),
                request.IsFavourite))
            .Execute(query => Mediator.Send(query))
            .ResolveResponse();

    /// <summary>
    /// Disables the notifications for a specific project for the current user.
    /// </summary>
    /// <param name="projectId">The id of the project for which notifications will be disabled.</param>
    /// <returns>
    /// HTTP response with status code:
    /// 204 - on success.
    /// 400 - on failure.
    /// 404 - when the project was not found.
    /// </returns>
    [HttpPost("{projectId:guid}/notifications/disable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IsProjectMemberPolicy)]
    public async Task<IActionResult> DisableProjectNotifications([FromRoute] Guid projectId) =>
        await Resolver<DisableUserNotificationsCommand>
            .For(new DisableUserNotificationsCommand(projectId.GuidConversion<ProjectId>()))
            .Execute(query => Mediator.Send(query))
            .ResolveResponse();

    /// <summary>
    /// Enables the notifications for a specific project for the current user.
    /// </summary>
    /// <param name="projectId">The id of the project for which notifications will be enabled.</param>
    /// <returns>
    /// HTTP response with status code:
    /// 204 - on success.
    /// 400 - on failure.
    /// 404 - when the project was not found.
    /// </returns>
    [HttpPost("{projectId:guid}/notifications/enable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IsProjectMemberPolicy)]
    public async Task<IActionResult> EnableProjectNotifications([FromRoute] Guid projectId) =>
        await Resolver<EnableUserNotificationsCommand>
            .For(new EnableUserNotificationsCommand(projectId.GuidConversion<ProjectId>()))
            .Execute(query => Mediator.Send(query))
            .ResolveResponse();
}