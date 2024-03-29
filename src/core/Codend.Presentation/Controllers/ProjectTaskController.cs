using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.Exceptions;
using Codend.Application.ProjectTasks.Commands.AssignUser;
using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Application.ProjectTasks.Commands.DeleteProjectTask;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Application.ProjectTasks.Queries.GetProjectTaskById;
using Codend.Contracts;
using Codend.Contracts.Requests;
using Codend.Contracts.Requests.ProjectTasks.Create;
using Codend.Contracts.Requests.ProjectTasks.Update;
using Codend.Contracts.Responses.ProjectTask;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask.Bugfix;
using Codend.Presentation.Extensions;
using Codend.Presentation.Infrastructure;
using Codend.Presentation.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

/// <summary>
/// Controller containing endpoints associated with <see cref="BaseProjectTask"/> and it's derived entities management.
/// </summary>
[Route("api/projects/{projectId:guid}/tasks")]
[Authorize(ProjectOperations.IsProjectMemberPolicy)]
public class ProjectTaskController : ApiController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectTaskController"/> class.
    /// </summary>
    public ProjectTaskController(IMediator mediator) : base(mediator)
    {
    }

    #region Private methods

    private async Task<IActionResult> UpdateTask<TCommand>(TCommand command)
        where TCommand : ICommand, IUpdateProjectTaskCommand =>
        await Resolver<TCommand>
            .For(command)
            .Execute(req => Mediator.Send(req))
            .ResolveResponse();

    #endregion

    #region Shared methods

    /// <summary>
    /// Deletes project task with given <paramref name="projectTaskId"/>.
    /// </summary>
    /// <param name="projectId">Id of the project to which the task belongs.</param>
    /// <param name="projectTaskId">Id of the project task that will be deleted.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 404 on failure
    /// </returns>
    [HttpDelete("{projectTaskId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(ProjectOperations.IsProjectMemberPolicy)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid projectId,
        [FromRoute] Guid projectTaskId) =>
        await Resolver<DeleteProjectTaskCommand>
            .For(new DeleteProjectTaskCommand(projectTaskId.GuidConversion<ProjectTaskId>()))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Assigns project member with id <paramref name="assigneeId"/> to task with id <paramref name="projectTaskId"/>.
    /// </summary>
    /// <param name="projectId">Id of the project to which the task belongs.</param>
    /// <param name="projectTaskId">Id of the project task to which the user will be assigned.</param>
    /// <param name="assigneeId">Id of the user that will be assigned.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 on userId failure with error response
    /// - 404 on failure
    /// </returns>
    [Route("{projectTaskId:guid}/assign/{assigneeId:guid}")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignUser(
        [FromRoute] Guid projectId,
        [FromRoute] Guid projectTaskId,
        [FromRoute] Guid assigneeId) =>
        await Resolver<AssignUserCommand>
            .For(new AssignUserCommand(
                projectId.GuidConversion<ProjectId>(),
                projectTaskId.GuidConversion<ProjectTaskId>(),
                assigneeId.GuidConversion<UserId>()
            ))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Retrieves common data of project task with <paramref name="projectTaskId"/>.
    /// </summary>
    /// <param name="projectId">Id of the project to which the task belongs.</param>
    /// <param name="projectTaskId">Id of the project task which data will be retrieved.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 200 on success with project task data
    /// - 404 on failure
    /// </returns>
    [HttpGet("{projectTaskId:guid}")]
    [ProducesResponseType(typeof(BaseProjectTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid projectId,
        [FromRoute] Guid projectTaskId) =>
        await Resolver<GetProjectTaskByIdQuery>
            .For(new GetProjectTaskByIdQuery(projectTaskId.GuidConversion<ProjectTaskId>()))
            .Execute(query => Mediator.Send(query))
            .ResolveResponse();

    #endregion

    #region BaseProjectTask

    /// <summary>
    /// Creates new BaseProjectTask entity.
    /// </summary>
    /// <param name="projectId">Id of the project where the task will be created.</param>
    /// <param name="request">The create base project task request which body
    /// includes required fields (name, priority) and
    /// optional fields (statusId, description, estimatedTime, dueDate, storyPoints, assigneeId, storyId).
    /// </param>
    /// <remarks>
    /// Valid priorities: [VeryHigh, High, Normal, Low, VeryLow]
    /// 
    /// Sample request:
    /// 
    ///     {
    ///         "name": "New project task name",
    ///         "priority": "Normal",
    ///         "statusId": "1f0c1930-50f4-4f17-8470-211b3a5cc873",
    ///         "description": "Not so long description",
    ///         "estimatedTime":{
    ///             "minutes": 45,
    ///             "hours": 3,
    ///             "days": 1
    ///         },
    ///         "dueDate": "2023-12-30T15:30:56.123Z",
    ///         "storyPoints": 35,
    ///         "assigneeId": "e405f337-4da0-4cce-818b-9231642c93fe",
    ///         "storyId": "a5a2e6b1-9b79-4d1d-b4f3-f0b5ac29ba42",
    ///         "sprintId": "a5a2e6b1-9b79-4d1d-b4f3-f0b5ac29ba42"
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 200 on success with newly created base project task id
    /// - 400 on failure with error response
    /// </returns>
    [HttpPost("base")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBaseTask(
        [FromRoute] Guid projectId,
        [FromBody] CreateBaseProjectTaskRequest request) =>
        await Resolver<CreateBaseProjectTaskCommand>
            .IfRequestNotNull(request)
            .ResolverFor(new CreateBaseProjectTaskCommand(
                new BaseProjectTaskCreateProperties(
                    projectId.GuidConversion<ProjectId>(),
                    request.Name,
                    request.Priority,
                    request.Description,
                    request.EstimatedTime.ToTimeSpan(),
                    request.DueDate,
                    request.StoryPoints,
                    request.AssigneeId.GuidConversion<UserId>(),
                    request.StoryId.GuidConversion<StoryId>()
                )
                {
                    StatusId = request.StatusId.GuidConversion<ProjectTaskStatusId>()
                },
                request.SprintId.GuidConversion<SprintId>()
            ))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();

    /// <summary>
    /// Updates BaseProjectTask entity with given <paramref name="projectTaskId"/>.
    /// </summary>
    /// <param name="projectId">Id of the project to which the task belongs.</param>
    /// <param name="projectTaskId">Id of the base project task that will be updated.</param>
    /// <param name="request">The update base project task request.</param>
    /// <remarks>
    /// Valid priorities: [VeryHigh, High, Normal, Low, VeryLow]
    /// 
    /// Sample request():
    /// 
    ///     {
    ///         "name": "new name",
    ///         "priority": "Low",
    ///         "statusId": "1f0c1930-50f4-4f17-8470-211b3a5cc873",
    ///         "description": {
    ///             "shouldUpdate": true,
    ///             "value": "New description"
    ///         },
    ///         "dueDate": {
    ///             "shouldUpdate": true,
    ///             "value": "2023-12-19T09:31:47.712Z"
    ///         },
    ///         "storyPoints": {
    ///             "shouldUpdate": true,
    ///             "value": 10
    ///         },
    ///         "statusId": {
    ///             "shouldUpdate": false,
    ///             "value": ""
    ///         },
    ///         "estimatedTime": {
    ///             "shouldUpdate": true,
    ///             "value": {
    ///                 "minutes": 15,
    ///                 "hours": 3,
    ///                 "days": 0
    ///             }
    ///         },
    ///         "assigneeId": {
    ///             "shouldUpdate": false,
    ///             "value": ""
    ///         },
    ///         "storyId": {
    ///             "shouldUpdate": false,
    ///             "value": ""
    ///         }
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 on failure with error response
    /// - 404 on failure
    /// </returns>
    [HttpPut("base/{projectTaskId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBaseTask(
        [FromRoute] Guid projectId,
        [FromRoute] Guid projectTaskId,
        [FromBody] UpdateBaseProjectTaskRequest request)
    {
        if (request is null)
        {
            throw new InvalidRequestException();
        }

        var command = new UpdateBaseProjectTaskCommand
        (
            projectId.GuidConversion<ProjectId>(),
            projectTaskId.GuidConversion<ProjectTaskId>(),
            request.Name,
            request.Priority,
            request.StatusId.GuidConversion<ProjectTaskStatusId>(),
            request.Description.HandleNull(),
            request.EstimatedTime.HandleNull().Convert(EstimatedTimeRequestExtensions.ToTimeSpan),
            request.DueDate.HandleNull(),
            request.StoryPoints.HandleNull(),
            request.AssigneeId.HandleNull().Convert(EntityIdExtensions.GuidConversion<UserId>),
            request.StoryId.HandleNull().Convert(EntityIdExtensions.GuidConversion<StoryId>)
        );

        return await UpdateTask(command);
    }

    #endregion

    #region BugfixProjectTask

    /// <summary>
    /// Creates new BugfixProjectTask entity.
    /// </summary>
    /// <param name="projectId">Id of the project where the task will be created.</param>
    /// <param name="request">The create bugfix project task request which body
    /// includes required fields (name, priority) and
    /// optional fields (statusId, description, estimatedTime, dueDate, storyPoints, assigneeId, storyId).
    /// </param>
    /// <remarks>
    /// Valid priorities: [VeryHigh, High, Normal, Low, VeryLow]
    /// 
    /// Sample request:
    /// 
    ///     {
    ///         "name": "New bugfix project task name",
    ///         "priority": "Normal",
    ///         "statusId": "1f0c1930-50f4-4f17-8470-211b3a5cc873",
    ///         "description": "Not so long description",
    ///         "estimatedTime":{
    ///             "minutes": 45,
    ///             "hours": 3,
    ///             "days": 1
    ///         },
    ///         "dueDate": "2023-12-30T15:30:56.123Z",
    ///         "storyPoints": 35,
    ///         "assigneeId": "e405f337-4da0-4cce-818b-9231642c93fe",
    ///         "storyId": "a5a2e6b1-9b79-4d1d-b4f3-f0b5ac29ba42",
    ///         "sprintId": "a5a2e6b1-9b79-4d1d-b4f3-f0b5ac29ba42"
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 200 on success with newly created bugfix project task id
    /// - 400 on failure with error response
    /// </returns>
    [HttpPost("bugfix")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBugfix(
        [FromRoute] Guid projectId,
        [FromBody] CreateBugfixProjectTaskRequest request) =>
        await Resolver<CreateBugfixProjectTaskCommand>
            .IfRequestNotNull(request)
            .ResolverFor(new CreateBugfixProjectTaskCommand(
                new BugfixProjectTaskCreateProperties(
                    projectId.GuidConversion<ProjectId>(),
                    request.Name,
                    request.Priority,
                    request.Description,
                    request.EstimatedTime.ToTimeSpan(),
                    request.DueDate,
                    request.StoryPoints,
                    request.AssigneeId.GuidConversion<UserId>(),
                    request.StoryId.GuidConversion<StoryId>()
                )
                {
                    StatusId = request.StatusId.GuidConversion<ProjectTaskStatusId>(),
                },
                request.SprintId.GuidConversion<SprintId>()
            ))
            .Execute(command => Mediator.Send(command))
            .ResolveResponse();


    /// <summary>
    /// Updates BugfixProjectTask entity with given <paramref name="projectTaskId"/>.
    /// </summary>
    /// <param name="projectId">Id of the project to which the task belongs.</param>
    /// <param name="projectTaskId">Id of the bugfix project task that will be updated.</param>
    /// <param name="request">The update bugfix project task request.</param>
    /// <remarks>
    /// Valid priorities: [VeryHigh, High, Normal, Low, VeryLow]
    /// 
    /// Sample request():
    /// 
    ///     {
    ///         "name": "new name",
    ///         "priority": "Low",
    ///         "statusId": "1f0c1930-50f4-4f17-8470-211b3a5cc873",
    ///         "description": {
    ///             "shouldUpdate": true,
    ///             "value": "New description"
    ///         },
    ///         "dueDate": {
    ///             "shouldUpdate": true,
    ///             "value": "2023-12-19T09:31:47.712Z"
    ///         },
    ///         "storyPoints": {
    ///             "shouldUpdate": true,
    ///             "value": 10
    ///         },
    ///         "estimatedTime": {
    ///             "shouldUpdate": true,
    ///             "value": {
    ///                 "minutes": 15,
    ///                 "hours": 3,
    ///                 "days": 0
    ///             }
    ///         },
    ///         "assigneeId": {
    ///             "shouldUpdate": false,
    ///             "value": ""
    ///         },
    ///         "storyId": {
    ///             "shouldUpdate": false,
    ///             "value": ""
    ///         }
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 on failure with error response
    /// - 404 on failure
    /// </returns>
    [HttpPut("bugfix/{projectTaskId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBugfixTask(
        [FromRoute] Guid projectId,
        [FromRoute] Guid projectTaskId,
        [FromBody] UpdateBugfixProjectTaskRequest request)
    {
        if (request is null)
        {
            throw new InvalidRequestException();
        }

        var command = new UpdateBugfixProjectTaskCommand
        (
            projectId.GuidConversion<ProjectId>(),
            projectTaskId.GuidConversion<ProjectTaskId>(),
            request.Name,
            request.Priority,
            request.StatusId.GuidConversion<ProjectTaskStatusId>(),
            request.Description.HandleNull(),
            request.EstimatedTime.HandleNull().Convert(EstimatedTimeRequestExtensions.ToTimeSpan),
            request.DueDate.HandleNull(),
            request.StoryPoints.HandleNull(),
            request.AssigneeId.HandleNull().Convert(EntityIdExtensions.GuidConversion<UserId>),
            request.StoryId.HandleNull().Convert(EntityIdExtensions.GuidConversion<StoryId>)
        )
        {
            TaskId = projectTaskId.GuidConversion<ProjectTaskId>()
        };

        return await UpdateTask(command);
    }

    #endregion
}