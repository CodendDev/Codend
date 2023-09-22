using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;

namespace Codend.Application.ProjectTaskStatuses.Commands.CreateProjectTaskStatus;

/// <summary>
/// Command used for creating new <see cref="ProjectTaskStatus"/>.
/// </summary>
/// <param name="Name">Status name.</param>
/// <param name="ProjectId">ProjectId.</param>
public sealed record CreateProjectTaskStatusCommand
(
    string Name,
    Guid ProjectId
) : ICommand<Guid>;