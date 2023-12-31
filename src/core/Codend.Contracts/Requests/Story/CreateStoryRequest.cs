﻿namespace Codend.Contracts.Requests.Story;

/// <summary>
/// Represents create story request.
/// </summary>
/// <param name="Name">Name of the story.</param>
/// <param name="Description">Description of the story.</param>
/// <param name="EpicId">EpicId of the story.</param>
/// <param name="StatusId">Story status.</param>
/// <param name="SprintId">Id of the sprint to which epic will be assigned.</param>;
public record CreateStoryRequest(
    string Name,
    string Description,
    Guid? EpicId,
    Guid? StatusId,
    Guid? SprintId
);