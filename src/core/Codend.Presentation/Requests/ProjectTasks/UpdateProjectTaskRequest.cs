using Codend.Contracts;
using Codend.Contracts.Common;
using Codend.Contracts.ProjectTasks;
using Codend.Shared;

namespace Codend.Presentation.Requests.ProjectTasks;

public class UpdateProjectTaskRequest : IUpdateProjectTaskRequest
{
    public Guid TaskId { get; set; }

    public ShouldUpdateBinder<string>? _Name { get; set; }
    public IShouldUpdate<string>? Name => _Name;

    public ShouldUpdateBinder<string>? _Priority { get; set; }
    public IShouldUpdate<string>? Priority => _Priority;

    public ShouldUpdateBinder<string?>? _Description { get; set; }
    public IShouldUpdate<string?>? Description => _Description;

    public ShouldUpdateBinder<DateTime?>? _DueDate { get; set; }
    public IShouldUpdate<DateTime?>? DueDate => _DueDate;


    public ShouldUpdateBinder<uint?>? _StoryPoints { get; set; }
    public IShouldUpdate<uint?>? StoryPoints => _StoryPoints;

    public ShouldUpdateBinder<Guid>? _StatusId { get; set; }
    public IShouldUpdate<Guid>? StatusId => _StatusId;

    public ShouldUpdateBinder<EstimatedTimeRequest>? _EstimatedTime { get; set; }
    public IShouldUpdate<EstimatedTimeRequest>? EstimatedTime => _EstimatedTime;

    public ShouldUpdateBinder<Guid?>? _AssigneeId { get; set; }
    public IShouldUpdate<Guid?>? AssigneeId => _AssigneeId;
}