﻿using Codend.Domain.Entities.ProjectTask.Abstractions;
using FluentResults;

namespace Codend.Domain.Entities.ProjectTask.Bugfix;

public class BugfixProjectTask :
    ProjectTaskBase,
    IProjectTaskCreator<BugfixProjectTask, BugfixProjectTaskCreateProperties>
{
    private BugfixProjectTask(ProjectTaskId id) : base(id)
    {
    }

    public static Result<BugfixProjectTask> Create(BugfixProjectTaskCreateProperties properties, UserId ownerId)
    {
        var task = new BugfixProjectTask(new ProjectTaskId(Guid.NewGuid()));
        var result = task.PopulateBaseProperties(properties, ownerId);

        if (result.IsFailed)
        {
            return result.ToResult();
        }

        return Result.Ok(task);
    }
}