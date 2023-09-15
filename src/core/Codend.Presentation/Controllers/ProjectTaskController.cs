using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.AssignUser;
using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;
using Codend.Application.ProjectTasks.Commands.DeleteProjectTask;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Application.ProjectTasks.Queries.GetProjectTaskById;
using Codend.Contracts;
using Codend.Contracts.Abstractions;
using Codend.Contracts.Requests;
using Codend.Contracts.Requests.ProjectTasks.Update;
using Codend.Contracts.Responses.ProjectTask;
using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Presentation.Infrastructure;
using Codend.Presentation.Requests.Abstractions;
using Codend.Presentation.Requests.ProjectTasks.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codend.Presentation.Controllers;

/// <summary>
/// Controller containing endpoints associated with <see cref="BaseProjectTask"/> and it's derived entities management.
/// </summary>
[Route("api/task")]
[AllowAnonymous]
public class ProjectTaskController : ApiController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectTaskController"/> class.
    /// </summary>
    public ProjectTaskController(IMediator mediator) : base(mediator)
    {
    }

    #region Private methods

    private async Task<IActionResult> CreateTask<TRequest, TCommand>(TRequest request)
        where TRequest : IMapRequestToCommand<TCommand, Guid>
        where TCommand : ICommand<Guid>, ICreateProjectTaskCommand
    {
        var command = request.MapToCommand();

        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return BadRequest(response.MapToApiErrorsResponse());
    }

    private async Task<IActionResult> UpdateTask<TCommand>(TCommand command)
        where TCommand : ICommand, IUpdateProjectTaskCommand
    {
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return NoContent();
        }

        if (response.HasError<DomainErrors.ProjectTaskErrors.ProjectTaskNotFound>())
        {
            return NotFound();
        }

        return BadRequest(response.MapToApiErrorsResponse());
    }

    #endregion

    #region Shared methods

    /// <summary>
    /// Deletes project task with given <paramref name="projectTaskId"/>.
    /// </summary>
    /// <param name="projectTaskId">Id of the project task that will be deleted.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 404 on failure
    /// </returns>
    [HttpDelete("{projectTaskId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid projectTaskId)
    {
        var command = new DeleteProjectTaskCommand(projectTaskId);
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return NoContent();
        }

        return NotFound();
    }

    /// <summary>
    /// Assigns project member with id <paramref name="userId"/> to task with id <paramref name="projectTaskId"/>.
    /// </summary>
    /// <param name="projectTaskId">Id of the project task to which the user will be assigned.</param>
    /// <param name="userId">Id of the user that will be assigned.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 on userId failure with error response
    /// - 404 on failure
    /// </returns>
    [Route("{projectTaskId:guid}/assign/{userId:guid}")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignUser([FromRoute] Guid projectTaskId, [FromRoute] Guid userId)
    {
        var command = new AssignUserCommand(projectTaskId, userId);
        var response = await Mediator.Send(command);
        if (response.IsSuccess)
        {
            return Ok();
        }

        if (response.HasError<DomainErrors.ProjectTaskErrors.InvalidAssigneeId>())
        {
            return BadRequest(response.MapToApiErrorsResponse());
        }

        return NotFound();
    }

    /// <summary>
    /// Retrieves common data of project task with <paramref name="projectTaskId"/>.
    /// </summary>
    /// <param name="projectTaskId">Id of the project task which data will be retrieved.</param>
    /// <returns>
    /// HTTP response with status code:
    /// - 200 on success with project task data
    /// - 404 on failure
    /// </returns>
    [HttpGet("{projectTaskId:guid}")]
    [ProducesResponseType(typeof(BaseProjectTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid projectTaskId)
    {
        var query = new GetProjectTaskByIdQuery(projectTaskId);
        var response = await Mediator.Send(query);
        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return NotFound();
    }

    #endregion

    #region BaseProjectTask

    /// <summary>
    /// Creates new BaseProjectTask entity.
    /// </summary>
    /// <param name="request">The create base project task request which body
    /// includes required fields (name, priority, statusId) and
    /// optional fields (description, estimatedTime, dueDate, storyPoints, assigneeId, storyId).
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
    ///             "days": 1,
    ///         }
    ///         "dueDate": "2023-12-30T15:30:56.123Z",
    ///         "storyPoints": 35,
    ///         "assigneeId": "e405f337-4da0-4cce-818b-9231642c93fe",
    ///         "storyId": "a5a2e6b1-9b79-4d1d-b4f3-f0b5ac29ba42"
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 200 on success with newly created base project task id
    /// - 400 on failure with error response
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBaseTask([FromBody] CreateBaseProjectTaskRequest request)
        => await CreateTask<CreateBaseProjectTaskRequest, CreateBaseProjectTaskCommand>(request);

    /// <summary>
    /// Updates BaseProjectTask entity with given <paramref name="projectTaskId"/>.
    /// </summary>
    /// <param name="projectTaskId">Id of the base project task that will be updated.</param>
    /// <param name="request">The update base project task request.</param>
    /// <remarks>
    /// Valid priorities: [VeryHigh, High, Normal, Low, VeryLow]
    /// TODO:replace with correct sample request
    /// Sample request():
    /// 
    ///     {
    ///         "name": "New project task name",
    ///         "priority": "Normal",
    ///         "statusId": "1f0c1930-50f4-4f17-8470-211b3a5cc873",
    ///         "description": "Not so long description",
    ///         "estimatedTime":{
    ///             "minutes": 45,
    ///             "hours": 3,
    ///             "days": 1,
    ///         }
    ///         "dueDate": "2023-12-30T15:30:56.123Z",
    ///         "storyPoints": 35,
    ///         "assigneeId": "e405f337-4da0-4cce-818b-9231642c93fe",
    ///         "storyId": "a5a2e6b1-9b79-4d1d-b4f3-f0b5ac29ba42"
    ///     }
    /// </remarks>
    /// <returns>
    /// HTTP response with status code:
    /// - 204 on success
    /// - 400 on failure with error response
    /// - 404 on failure
    /// </returns>
    [HttpPut("{projectTaskId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBaseTask(
        [FromRoute] Guid projectTaskId,
        [FromBody] UpdateBaseProjectTaskRequest request)
    {
        var command = new UpdateBaseProjectTaskCommand
        (
            request.Name.HandleNull(),
            request.Priority.HandleNull(),
            request.StatusId.HandleNull().HasConversion(guid => new ProjectTaskStatusId(guid)),
            request.Description.HandleNull(),
            request.EstimatedTime.HandleNull().HasConversion(EstimatedTimeRequestExtensions.ToTimeSpan),
            request.DueDate.HandleNull(),
            request.StoryPoints.HandleNull(),
            request.AssigneeId.HandleNull().HasConversion(EntityIdExtensions.ToKeyGuid<UserId>),
            request.StoryId.HandleNull().HasConversion(EntityIdExtensions.ToKeyGuid<StoryId>)
        )
        {
            TaskId = new ProjectTaskId(projectTaskId)
        };

        return await UpdateTask(command);
    }

    #endregion

    #region BugfixProjectTask

    /// <summary>
    /// Creates new BugfixProjectTask entity.
    /// </summary>
    /// <param name="request">The create bugfix project task request which body
    /// includes required fields (name, priority, statusId) and
    /// optional fields (description, estimatedTime, dueDate, storyPoints, assigneeId, storyId).
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
    ///             "days": 1,
    ///         }
    ///         "dueDate": "2023-12-30T15:30:56.123Z",
    ///         "storyPoints": 35,
    ///         "assigneeId": "e405f337-4da0-4cce-818b-9231642c93fe",
    ///         "storyId": "a5a2e6b1-9b79-4d1d-b4f3-f0b5ac29ba42"
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
    public async Task<IActionResult> CreateBugfix([FromBody] CreateBugfixProjectTaskRequest request)
        => await CreateTask<CreateBugfixProjectTaskRequest, CreateBugfixProjectTaskCommand>(request);


    /// <summary>
    /// Updates BugfixProjectTask entity with given <paramref name="projectTaskId"/>.
    /// </summary>
    /// <param name="projectTaskId">Id of the bugfix project task that will be updated.</param>
    /// <param name="request">The update bugfix project task request.</param>
    /// <remarks>
    /// Valid priorities: [VeryHigh, High, Normal, Low, VeryLow]
    /// TODO:replace with correct sample request
    /// Sample request():
    /// 
    ///     {
    ///         "name": "New project task name",
    ///         "priority": "Normal",
    ///         "statusId": "1f0c1930-50f4-4f17-8470-211b3a5cc873",
    ///         "description": "Not so long description",
    ///         "estimatedTime":{
    ///             "minutes": 45,
    ///             "hours": 3,
    ///             "days": 1,
    ///         }
    ///         "dueDate": "2023-12-30T15:30:56.123Z",
    ///         "storyPoints": 35,
    ///         "assigneeId": "e405f337-4da0-4cce-818b-9231642c93fe",
    ///         "storyId": "a5a2e6b1-9b79-4d1d-b4f3-f0b5ac29ba42"
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
    public async Task<IActionResult> UpdateBugfixTask([FromRoute] Guid projectTaskId,
        [FromBody] UpdateBugfixProjectTaskRequest request)
    {
        var command = new UpdateBaseProjectTaskCommand
        (
            request.Name.HandleNull(),
            request.Priority.HandleNull(),
            request.StatusId.HandleNull().HasConversion(guid => new ProjectTaskStatusId(guid)),
            request.Description.HandleNull(),
            request.EstimatedTime.HandleNull().HasConversion(EstimatedTimeRequestExtensions.ToTimeSpan),
            request.DueDate.HandleNull(),
            request.StoryPoints.HandleNull(),
            request.AssigneeId.HandleNull().HasConversion(EntityIdExtensions.ToKeyGuid<UserId>),
            request.StoryId.HandleNull().HasConversion(EntityIdExtensions.ToKeyGuid<StoryId>)
        )
        {
            TaskId = new ProjectTaskId(projectTaskId)
        };

        return await UpdateTask(command);
    }

    #endregion
}