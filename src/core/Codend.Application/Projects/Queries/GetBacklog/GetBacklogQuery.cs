using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Application.Core.Abstractions.Services;
using Codend.Application.Projects.Queries.GetProjects;
using Codend.Contracts.Responses.Backlog;
using Codend.Domain.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Codend.Application.Projects.Queries.GetBacklog;

/// <summary>
/// Query for retrieving all backlog information for project, including tasks, epic and stories.
/// </summary>
/// <param name="ProjectId">Id of the project for which backlog will be returned.</param>
public sealed record GetBacklogQuery(ProjectId ProjectId) : IQuery<BacklogResponse>;

/// <summary>
/// <see cref="GetBacklogQuery"/> Handler.
/// </summary>
public class GetBacklogQueryHandler : IQueryHandler<GetBacklogQuery, BacklogResponse>
{
    private readonly IQueryableSets _context;
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProjectsQueryHandler"/> class.
    /// </summary>
    public GetBacklogQueryHandler(
        IQueryableSets context,
        IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    /// <inheritdoc />
    public async Task<Result<BacklogResponse>> Handle(GetBacklogQuery query,
        CancellationToken cancellationToken)
    {
        var statuses = await _context.Queryable<ProjectTaskStatus>()
            .Where(status => status.ProjectId == query.ProjectId)
            .ToDictionaryAsync(status => status.Id.Value, status => status.Name.Value, cancellationToken);

        var dataSeparated = new[]
        {
            await FetchProjectTasks(query.ProjectId, statuses, cancellationToken),
            await FetchStories(query.ProjectId, statuses, cancellationToken),
            await FetchEpics(query.ProjectId, statuses, cancellationToken),
        };

        var data = dataSeparated
            .SelectMany(coll => coll)
            .OrderByDescending(x => x.CreatedOn);

        return Result.Ok(new BacklogResponse(data));
    }

    private async Task<IEnumerable<BacklogTaskResponse>> FetchProjectTasks(
        ProjectId projectId,
        IReadOnlyDictionary<Guid, string> statuses,
        CancellationToken cancellationToken)
    {
        var projectTasks = await _context.Queryable<BaseProjectTask>()
            .Where(baseProjectTask => baseProjectTask.ProjectId == projectId)
            .Select(projTask => new
            {
                Id = projTask.Id.Value,
                Name = projTask.Name.Value,
                TaskType = projTask.TaskType,
                StatusId = projTask.StatusId.Value,
                AssigneeId = projTask.AssigneeId,
                CreatedOn = projTask.CreatedOn
            })
            .ToListAsync(cancellationToken);

        var userIds = projectTasks
            .Where(x => x.AssigneeId is not null)
            .Select(x => x.AssigneeId!).ToList();

        var users = await _userService.GetUsersByIdsAsync(userIds);

        return projectTasks.Select(x => new BacklogTaskResponse(
            x.Id,
            x.Name,
            x.TaskType,
            statuses[x.StatusId],
            users.SingleOrDefault(usr => usr.Id == x.AssigneeId?.Value)?.ImageUrl,
            x.CreatedOn
        ));
    }

    private async Task<IEnumerable<BacklogTaskResponse>> FetchStories(
        ProjectId projectId,
        IReadOnlyDictionary<Guid, string> statuses,
        CancellationToken cancellationToken)
    {
        var stories = await _context.Queryable<Story>()
            .Where(story => story.ProjectId == projectId)
            .Select(story => new
            {
                Id = story.Id.Value,
                Name = story.Name.Value,
                TaskType = story.SprintTaskType,
                StatusId = story.StatusId.Value,
                CreatedOn = story.CreatedOn
            })
            .ToListAsync(cancellationToken);

        return stories.Select(x => new BacklogTaskResponse(
            x.Id,
            x.Name,
            x.TaskType,
            statuses[x.StatusId],
            null,
            x.CreatedOn
        ));
    }

    private async Task<IEnumerable<BacklogTaskResponse>> FetchEpics(
        ProjectId projectId,
        IReadOnlyDictionary<Guid, string> statuses,
        CancellationToken cancellationToken)
    {
        var epics = await _context.Queryable<Epic>()
            .Where(epic => epic.ProjectId == projectId)
            .Select(epic => new
            {
                Id = epic.Id.Value,
                Name = epic.Name.Value,
                TaskType = epic.SprintTaskType,
                StatusId = epic.StatusId.Value,
                CreatedOn = epic.CreatedOn
            })
            .ToListAsync(cancellationToken);

        return epics.Select(x => new BacklogTaskResponse(
            x.Id,
            x.Name,
            x.TaskType,
            statuses[x.StatusId],
            null,
            x.CreatedOn
        ));
    }
}