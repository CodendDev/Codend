﻿using Codend.Contracts.Abstractions;

namespace Codend.Contracts.Requests.ProjectTasks.Update;

public interface IUpdateBaseProjectTaskRequest
{
    public Guid TaskId { get; }
    IShouldUpdate<string>? Name { get; }
    IShouldUpdate<string>? Priority { get; }
    IShouldUpdate<string?>? Description { get; }
    IShouldUpdate<DateTime?>? DueDate { get; }
    IShouldUpdate<uint?>? StoryPoints { get; }
    IShouldUpdate<Guid>? StatusId { get; }
    IShouldUpdate<EstimatedTimeRequest>? EstimatedTime { get; }
    IShouldUpdate<Guid?>? AssigneeId { get; }
}