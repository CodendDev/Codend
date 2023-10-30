using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;

namespace Codend.Contracts.Requests.Sprint;

/// <summary>
/// Sprint task request.
/// </summary>
/// <param name="Id">Task id.</param>
/// <param name="Type">Task type.</param>
public record SprintTaskRequest
(
    Guid Id,
    string Type
)
{
    /// <summary>
    /// Converts Guid to <see cref="ISprintTaskId"/> depending on request type.
    /// </summary>
    /// <returns>Request Id as <see cref="ISprintTaskId"/>.</returns>
    /// <exception cref="Exception">Thrown if type doesn't match any  <see cref="ISprintTaskId"/> type.</exception>
    public ISprintTaskId MapToSprintTaskId()
    {
        return Type.ToLower() switch
        {
            "task" => Id.GuidConversion<ProjectTaskId>(),
            "story" => Id.GuidConversion<StoryId>(),
            "epic" => Id.GuidConversion<EpicId>(),
            _ => throw new Exception(),
        };
    }
};